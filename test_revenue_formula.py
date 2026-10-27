"""Offline tests for the revenue item-total mapping. No database or AI calls."""
import json
import unittest

import app


class RoundingExpressionTests(unittest.TestCase):
    def test_trunc_has_single_argument_for_mongodb_34(self):
        expression = app.round2_mongo({"$literal": 1.005})
        trunc = expression["$divide"][0]["$trunc"]
        self.assertIsInstance(trunc, list)
        self.assertEqual(len(trunc), 1, "MongoDB 3.4 rejects $trunc with a second argument")
        self.assertEqual(expression["$divide"][1], 100)

    def test_no_round_operator(self):
        expression = json.dumps(app.round2_mongo({"$literal": 1.005}))
        self.assertNotIn("$round", expression)

    def test_half_cent_nudges_both_directions(self):
        add = app.round2_mongo({"$literal": 1.005})["$divide"][0]["$trunc"][0]["$add"]
        condition = add[1]["$cond"]
        self.assertEqual(condition[1], 0.500000001)
        self.assertEqual(condition[2], -0.500000001)


class ItemTotalFormulaTests(unittest.TestCase):
    def test_formula_uses_every_native_component(self):
        expression = json.dumps(app.COMPUTED_VALUES["@item_total"])
        for field in (
            "ItensBase.PrecoUnitario",
            "ItensBase.Quantidade",
            "ItensBase.DescontoDigitado",
            "ItensBase.DescontoProporcional",
            "ItensBase.OutrasDespesasProporcional",
            "ItensBase.OutrasDespesasDigitado",
            "ItensBase.Percentual.ValorSt",
            "ItensBase.IPI.Valor",
            "ItensBase.Frete",
            "ItensBase.Seguro",
        ):
            self.assertIn(field, expression)

    def test_gross_is_computed_before_discounts(self):
        subtract = app.COMPUTED_VALUES["@item_total"]["$subtract"]
        added, removed = subtract
        self.assertIn("ItensBase.DescontoDigitado", json.dumps(removed))
        self.assertIn("ItensBase.PrecoUnitario", json.dumps(added))

    def test_price_and_quantity_prefer_computed_total(self):
        names = [
            "DataHoraEmissao",
            "ItensBase[].PrecoUnitario",
            "ItensBase[].Quantidade",
            "ItensBase[].TotalLiquido",
            "ItensBase[].ProdutoServico.Descricao",
        ]
        self.assertEqual(app.suggest_fields(names)["value"], "@item_total")

    def test_native_field_is_used_when_price_is_missing(self):
        names = ["DataHoraEmissao", "ItensBase[].TotalLiquido"]
        self.assertEqual(app.suggest_fields(names)["value"], "ItensBase[].TotalLiquido")


if __name__ == "__main__":
    unittest.main()
