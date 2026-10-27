"""1.0 regression suite: plans, persistent conversations, evidence and scheduling."""
from __future__ import annotations

import json
import math
import os
from pathlib import Path
import sqlite3
import tempfile
import time
import unittest
from contextlib import closing
from unittest.mock import patch, MagicMock

from fastapi import HTTPException
import app
from insight_plans import Plan, infer_plan
from insight_conversation import reply, ConversationRequest
from insight_engine import execute_plan
import insight_store as store

ROOT = Path(__file__).parent
FIXTURES = ROOT / "tests" / "fixtures"


class PlanBenchmark(unittest.TestCase):
    def test_question_benchmark(self):
        cases = json.loads((FIXTURES / "question-benchmark.json").read_text(encoding="utf-8"))
        for case in cases:
            with self.subTest(question=case["q"]):
                base=Plan(start="2026-10-05",end="2026-10-05",kind="investigation" if case.get("context") else "revenue",
                          seller_query="2" if case.get("context") else None)
                plan,prompt=infer_plan(case["q"],base,case.get("context",False))
                if case.get("clarification"):
                    self.assertIsNotNone(prompt)
                    self.assertEqual(prompt["field"],case["clarification"])
                else:
                    self.assertIsNone(prompt)
                    values=plan.model_dump(mode="json")
                    for k,v in case["expect"].items():self.assertEqual(values[k],v)
                    if case.get("comparison"):self.assertIsNotNone(plan.comparison)

    def test_typed_plan_rejects_query_injection(self):
        for data in [{"movement_group":"$where"},{"seller_query":{"$ne":None}},{"offset":-1},{"page_size":5001},
                     {"profile":"run_arbitrary_code"},{"comparison":{"start":"2026-10-05","end":"2026-10-01"}}]:
            with self.subTest(data=data),self.assertRaises(ValueError):Plan(start="2026-10-05",end="2026-10-05",**data)

    def test_mapped_fields_cannot_be_mongo_operators(self):
        for field in ["$where","Empresa.$where","$function","a..b",".Empresa","ItensBase[$].Valor"]:
            with self.subTest(field=field),self.assertRaises(HTTPException):app.escape_field(field)
        self.assertEqual(app.escape_field("ItensBase[].ProdutoServico.Descricao"),"$ItensBase.ProdutoServico.Descricao")

    def test_invalid_date_requires_correction(self):
        with self.assertRaises(HTTPException):infer_plan("diferença em 31/02/2026",Plan(start="2026-10-05",end="2026-10-05"),False)

    def test_period_comparison_keeps_seller_and_company(self):
        base=Plan(start="2026-10-05",end="2026-10-05",seller_query="2",company_value="Empresa A",kind="investigation")
        plan,prompt=infer_plan("Compare com o dia anterior",base,True)
        self.assertIsNone(prompt)
        self.assertEqual(plan.comparison.seller_query,"2")
        self.assertEqual(plan.comparison.company_value,"Empresa A")


def evaluate(expression, item):
    if isinstance(expression,str) and expression.startswith("$ItensBase."):
        value=item
        for key in expression[len("$ItensBase."):].split("."):
            value=value.get(key) if isinstance(value,dict) else None
        return value
    if isinstance(expression,list):return [evaluate(v,item) for v in expression]
    if not isinstance(expression,dict):return expression
    op,args=next(iter(expression.items()))
    if op=="$ifNull":
        value=evaluate(args[0],item)
        return value if value is not None else evaluate(args[1],item)
    if op=="$cond":return evaluate(args[1] if evaluate(args[0],item) else args[2],item)
    values=evaluate(args,item)
    if op=="$add":return sum(values)
    if op=="$multiply":return math.prod(values)
    if op=="$subtract":return values[0]-values[1]
    if op=="$divide":return values[0]/values[1]
    if op=="$gte":return values[0]>=values[1]
    if op=="$trunc":return math.trunc(values[0])
    raise AssertionError(op)


class GoldenFinancialFixtures(unittest.TestCase):
    def test_synthetic_sales_against_mongo_expression(self):
        for case in json.loads((FIXTURES/"synthetic-sales.json").read_text(encoding="utf-8")):
            with self.subTest(case=case["name"]):
                items=[i for i in case["items"] if not i.get("Cancelado")]
                total=sum(evaluate(app.COMPUTED_VALUES["@item_total"],i) for i in items)
                mine=sum(evaluate(app.COMPUTED_VALUES["@item_total"],i) for i in items if i.get("VendedorReferencia")==case["seller"])
                values={"items":total,"seller":mine,"other":total-mine,"difference":total-case["payments"]}
                for key,value in case["expected"].items():self.assertAlmostEqual(values[key],value,places=2)


class LocalStateTests(unittest.TestCase):
    def setUp(self):
        self.folder=tempfile.TemporaryDirectory(dir=ROOT/".release-build")
        self.env=patch.dict(os.environ,{"INSIGHT_DATA_DIR":self.folder.name,"INSIGHT_SCHEDULER":"0"})
        self.env.start()

    def tearDown(self):
        self.env.stop();self.folder.cleanup()

    def test_saved_rename_preserves_snapshot_and_rejects_invalid_title(self):
        from insight_workspace import SavedRequest, SavedTitle, save_investigation, rename_saved, saved_list
        from pydantic import ValidationError
        run_id=store.save_run({"kind":"revenue"},{"total":123.45})
        original=store.get_run(run_id)
        saved=save_investigation(SavedRequest(title='Original',run_id=run_id))
        response=rename_saved(saved['id'],SavedTitle(title='  Conferência São Paulo  '))
        self.assertEqual(response['title'],'Conferência São Paulo')
        self.assertEqual(saved_list()['saved'][0]['run_id'],run_id)
        self.assertEqual(store.get_run(run_id),original)
        for invalid in ['', '   ', 'x'*121]:
            with self.assertRaises(ValidationError):SavedTitle(title=invalid)
        with self.assertRaises(HTTPException) as missing:
            rename_saved('missing',SavedTitle(title='Novo'))
        self.assertEqual(missing.exception.status_code,404)
        self.assertEqual(saved_list()['saved'][0]['title'],'Conferência São Paulo')

    def test_context_and_clarification_persist_without_query(self):
        with patch("insight_conversation.execute_plan") as execute:
            response=reply(ConversationRequest(message="E ontem?"))
            self.assertEqual(response["status"],"clarification")
            execute.assert_not_called()
        _,session=store.get_conversation(response["conversation_id"])
        self.assertEqual(session["pending"]["clarification"]["field"],"kind")
        with patch("insight_conversation.execute_plan") as execute:
            again=reply(ConversationRequest(conversation_id=response["conversation_id"],action="refine"))
            self.assertEqual(again["status"],"clarification")
            execute.assert_not_called()

    def test_ambiguous_seller_produces_choices_without_saving_a_result(self):
        error=HTTPException(422,"Vendedor '2' é ambíguo")
        error.choices=[{"label":"Pessoa A","value":"a"},{"label":"Pessoa B","value":"b"}]
        with patch("insight_conversation.execute_plan",side_effect=error):
            response=reply(ConversationRequest(message="vendedor 2 em 05/10/2026"))
        self.assertEqual(len(response["clarification"]["options"]),2)
        self.assertEqual(store.read("SELECT count(*) n FROM runs")[0]["n"],0)

    def test_backup_reopens_as_valid_sqlite(self):
        key=store.save_run({"start":"2026-10-05"},{"rows":[]})
        path=store.backup_state()
        with closing(sqlite3.connect(path)) as db:self.assertEqual(db.execute("SELECT id FROM runs").fetchone()[0],key)

    def test_database_role_verification_rejects_root_and_unverified(self):
        from insight_connection import privileges
        client=MagicMock()
        client.admin.command.return_value={"authInfo":{"authenticatedUsers":[{}],"authenticatedUserPrivileges":[{"actions":["find","insert"]}]}}
        self.assertFalse(privileges(client)["verified_read_only"])
        client.admin.command.return_value={"authInfo":{"authenticatedUsers":[{}],"authenticatedUserPrivileges":[{"actions":["find","listCollections","listIndexes"]}]}}
        self.assertTrue(privileges(client)["verified_read_only"])
        client.admin.command.return_value={}
        self.assertFalse(privileges(client)["verified_read_only"])

    def test_export_formula_injection_is_escaped(self):
        from insight_exports import safe_text
        for text in ['=HYPERLINK("http://example.invalid")','+cmd','-cmd','@cmd','  =1+1']:
            self.assertTrue(safe_text(text).startswith("'"))

    def test_connection_save_preserves_previous_secret_and_configuration(self):
        from insight_connection import save_connection,ConnectionSettings
        folder=Path(self.folder.name)
        fields={name:getattr(app,name) for name in ["MONGO_URI","MONGO_DATABASE","MONGO_USERNAME","MONGO_AUTH_SOURCE","MONGO_PASSWORD_FILE","MODE"]}
        with patch("insight_connection.runtime_dir",return_value=folder),patch("insight_connection.test_connection",return_value={"connected":True}),patch.multiple(app,**fields):
            save_connection(ConnectionSettings(database="Test",username="reader",password="first-password"))
            old=app.MONGO_PASSWORD_FILE
            save_connection(ConnectionSettings(database="Test",username="reader",password="second-password"))
            self.assertNotEqual(app.MONGO_PASSWORD_FILE,old)
            self.assertEqual(Path(old).read_text().strip(),"first-password")
            self.assertIn(Path(old).name,(folder/".env.previous").read_text())
            self.assertNotIn("second-password",(folder/".env").read_text())

    def test_read_only_requirement_refuses_unverified_permissions(self):
        from insight_connection import enforce,_security_cache
        _security_cache.clear()
        client=MagicMock()
        with patch.dict(os.environ,{"INSIGHT_REQUIRE_READ_ONLY":"1"}),patch("insight_connection.privileges",return_value={"verified_read_only":False}):
            with self.assertRaises(HTTPException):enforce(client)
        client.close.assert_called_once()


class WorkspaceIntegration(LocalStateTests):
    @classmethod
    def setUpClass(cls):
        try:
            with app.mongo_client() as c:c.admin.command("ping")
        except Exception as exc:raise unittest.SkipTest(type(exc).__name__)

    def example(self):
        return reply(ConversationRequest(message="Diferença no vendedor 2 em 05/10/2026 de R$100,00"))

    def test_followups_preserve_date_and_change_seller(self):
        initial=self.example()
        follow=reply(ConversationRequest(conversation_id=initial["conversation_id"],message="E o vendedor 3?"))
        self.assertEqual(follow["plan"]["start"],"2026-10-05")
        self.assertNotEqual(follow["plan"]["seller_query"],initial["plan"]["seller_query"])
        next_day=reply(ConversationRequest(conversation_id=follow["conversation_id"],message="E no dia seguinte?"))
        self.assertEqual(next_day["plan"]["start"],"2026-10-06")
        self.assertEqual(next_day["plan"]["seller_query"],follow["plan"]["seller_query"])

    def test_clarification_answer_then_new_followup(self):
        first=reply(ConversationRequest(message="E ontem?"))
        result=reply(ConversationRequest(conversation_id=first["conversation_id"],answer="documents"))
        self.assertEqual(result["status"],"result")
        self.assertEqual(result["plan"]["start"],result["plan"]["end"])

    def test_comparison_matches_independent_executions(self):
        initial=self.example()
        compared=reply(ConversationRequest(conversation_id=initial["conversation_id"],message="Compare com o vendedor 3"))
        self.assertEqual(compared["result"]["totals"],initial["result"]["totals"])
        secondary=execute_plan(Plan(start="2026-10-05",end="2026-10-05",kind="investigation",seller_query="3"))
        metric=compared["result"]["comparison"]["metrics"]["Vendedor"]
        self.assertEqual(metric["comparison"],secondary["totals"]["Vendedor"])
        self.assertAlmostEqual(metric["difference"],metric["comparison"]-metric["base"],places=2)

    def test_search_and_paging_preserve_totals_and_stable_order(self):
        plan=Plan(start="2026-10-05",end="2026-10-05",kind="documents",seller_query="2",page_size=10)
        first=execute_plan(plan)
        second=execute_plan(plan.model_copy(update={"offset":10}))
        self.assertEqual(first["totals"],second["totals"])
        self.assertTrue(first["pagination"]["has_more"])
        self.assertNotEqual(first["rows"][0]["Numero"],second["rows"][0]["Numero"])
        searched=execute_plan(plan.model_copy(update={"search":"217242"}))
        self.assertEqual(searched["documents"],1)
        self.assertEqual(str(searched["rows"][0]["Numero"]),"217242")

    def test_native_profile_matches_known_month_and_excludes_wider_scope(self):
        native=execute_plan(Plan(start="2026-10-01",end="2026-10-26",kind="investigation",profile="native_nfce"))
        self.assertAlmostEqual(native["totals"]["Itens"],89813.23,places=2)
        self.assertEqual(native["plan"]["movement_group"],"nfce")
        self.assertTrue(all(r["Situacao"]=="Concluído" for r in native["rows"]))
        fiscal=execute_plan(Plan(start="2026-10-01",end="2026-10-26",kind="investigation",profile="fiscal_valid"))
        completed=execute_plan(Plan(start="2026-10-01",end="2026-10-26",kind="investigation",profile="fiscal_valid",status_filter="completed"))
        self.assertAlmostEqual(fiscal["totals"]["Itens"]-completed["totals"]["Itens"],97.93,places=2)

    def test_quality_checks_unpaid_and_shared_sales(self):
        quality=execute_plan(Plan(start="2026-10-05",end="2026-10-05",kind="quality"))
        self.assertGreaterEqual(quality["quality"]["counts"]["unpaid"],3)
        self.assertGreaterEqual(quality["quality"]["counts"]["shared"],2)
        filtered=execute_plan(Plan(start="2026-10-05",end="2026-10-05",kind="quality",issue="unpaid"))
        self.assertTrue(all(abs(r["Pagamentos"])<.01 for r in filtered["rows"]))
        native=execute_plan(Plan(start="2026-10-05",end="2026-10-05",kind="quality",profile="native_nfce"))
        self.assertEqual(native["plan"]["kind"],"quality")

    def test_saved_snapshot_is_not_recalculated_on_resume(self):
        from insight_workspace import resume,ResumeRequest
        initial=self.example()
        with patch("insight_conversation.execute_plan") as query:
            saved=resume(ResumeRequest(run_id=initial["run_id"]))
            query.assert_not_called()
        self.assertEqual(saved["result"]["provenance"]["source"],"saved_snapshot")
        self.assertEqual(saved["result"]["totals"],initial["result"]["totals"])

    def test_complete_excel_pdf_and_csv_match_result(self):
        from insight_exports import ExportRequest,start_export,get_job
        from openpyxl import load_workbook
        from pypdf import PdfReader
        initial=self.example()
        for fmt in ["xlsx","pdf","csv"]:
            with self.subTest(format=fmt):
                job=start_export(ExportRequest(run_id=initial["run_id"],format=fmt,complete=True))
                deadline=time.monotonic()+45
                while time.monotonic()<deadline:
                    current=get_job(job["id"])
                    if current["status"] in {"succeeded","failed"}:break
                    time.sleep(.1)
                self.assertEqual(current["status"],"succeeded",current.get("error"))
                self.assertEqual(current["records"],37)
                path=store.data_dir()/"exports"/current["filename"]
                if fmt=="xlsx":
                    workbook=load_workbook(path,read_only=True,data_only=False)
                    rows=list(workbook["Base - vendas"].values)
                    self.assertEqual(len(rows),38)
                    columns=rows[0]; other=columns.index("Outros/sem vendedor")
                    self.assertAlmostEqual(sum(float(r[other]) for r in rows[1:]),104.96,places=2)
                    workbook.close()
                elif fmt=="pdf":
                    reader=PdfReader(path);text="\n".join(p.extract_text() for p in reader.pages)
                    self.assertIn("104,96",text);self.assertIn("217242",text)
                else:self.assertIn("217242",path.read_text(encoding="utf-8-sig"))

    def test_schedule_persists_runs_and_deduplicates_alerts(self):
        from insight_schedule import ScheduleRequest,create,run_once
        initial=self.example()
        schedule=create(ScheduleRequest(run_id=initial["run_id"],title="Teste",period_policy="fixed",metric="Outros",threshold=100))
        self.assertFalse(schedule["enabled"])
        first=run_once(schedule["id"])
        self.assertEqual(first["last_status"],"succeeded")
        self.assertAlmostEqual(first["last_value"],104.96,places=2)
        run_once(schedule["id"])
        self.assertEqual(store.read("SELECT count(*) n FROM alerts")[0]["n"],1)


if __name__=="__main__":unittest.main(verbosity=2)
