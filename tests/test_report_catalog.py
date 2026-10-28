"""Static catalog contracts; no SuiteG6 execution or MongoDB connection."""
import importlib.util
import json
from pathlib import Path
import socket
import threading
import time
import unittest
from urllib.request import urlopen

ROOT = Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location('build_report_catalog', ROOT / 'tools/build-report-catalog.py')
builder = importlib.util.module_from_spec(spec)
spec.loader.exec_module(builder)


class ReportCatalogTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.catalog = json.loads((ROOT / 'static/report-catalog.json').read_text(encoding='utf-8'))

    def test_all_entries_have_unique_module_scoped_identity(self):
        reports = self.catalog['reports']
        self.assertEqual(len(reports), 358)
        self.assertEqual(len({r['id'] for r in reports}), len(reports))
        self.assertEqual(len({r['class'] for r in reports}), 330)
        self.assertEqual(sum(m['count'] for m in self.catalog['modules']), len(reports))

    def test_screenshot_modules_are_mapped_without_omitting_other_segments(self):
        modules = self.catalog['modules']
        self.assertEqual(len(modules), 40)
        self.assertEqual(sum(m['shown_in_reference'] for m in modules), 34)
        self.assertEqual(next(m['label'] for m in modules if m['id'] == 'Pagamento'), 'Contas a pagar')

    def test_research_is_not_advertised_as_executable(self):
        self.assertEqual(self.catalog['status'], 'in_progress')
        self.assertEqual(self.catalog['summary']['native_queries_implemented'], 356)
        self.assertEqual(self.catalog['summary']['native_print_layouts_reproduced'],0)
        from insight_finance import SPECS
        self.assertEqual(self.catalog['summary']['data_views_implemented'], len(SPECS))
        for report in self.catalog['reports']:
            self.assertEqual(report['execution']['status'], 'implemented_data_view' if report['id'] in SPECS else 'native_service_available' if report['sources'] else 'unavailable_in_installed_build')
            self.assertTrue(report['execution']['reason'])

    def test_documentation_exists_for_every_module_and_report(self):
        for module in self.catalog['modules']:
            text = (builder.GUIDE / (module['id'] + '.md')).read_text(encoding='utf-8')
            for report in self.catalog['reports']:
                if report['module'] == module['id']:
                    self.assertIn('`' + report['id'] + '`', text)
                    for ref in report['references']:
                        self.assertTrue((ROOT / ref).is_file(), ref)

    def test_literal_extractor_does_not_invent_mongo_mappings(self):
        extracted = builder.extract('Titulo = "Pessoa"; new XRBinding("Text", null, "Cliente.Nome"); '
                                    'x.Expression = "[Quantidade] * [Preco]"; x.Func = SummaryFunc.Sum;')
        self.assertEqual(extracted['parameters'], ['Pessoa'])
        self.assertEqual(extracted['bindings'], ['Cliente.Nome'])
        self.assertEqual(extracted['expressions'], ['[Quantidade] * [Preco]'])
        self.assertEqual(extracted['summaries'], ['Sum'])
        self.assertNotIn('collection', extracted)

    def test_page_and_catalog_are_served_without_database(self):
        from app import app
        import uvicorn
        with socket.socket() as sock:
            sock.bind(('127.0.0.1', 0))
            address = f'http://127.0.0.1:{sock.getsockname()[1]}'
            server = uvicorn.Server(uvicorn.Config(app, lifespan='off', log_level='error'))
            thread = threading.Thread(target=server.run, kwargs={'sockets': [sock]}, daemon=True)
            thread.start()
            try:
                deadline = time.monotonic() + 10
                while not server.started and thread.is_alive() and time.monotonic() < deadline:
                    time.sleep(.05)
                self.assertTrue(server.started)
                with urlopen(address + '/reports', timeout=5) as response:
                    self.assertEqual(response.status, 200)
                    self.assertIn('Catálogo em desenvolvimento', response.read().decode('utf-8'))
                with urlopen(address + '/static/report-catalog.json', timeout=5) as response:
                    self.assertEqual(response.status, 200)
                    self.assertEqual(json.load(response)['summary']['entries'], 358)
                with urlopen(address + '/finance-reports', timeout=5) as response:
                    self.assertEqual(response.status, 200)
                    self.assertIn('Consultar parcelas', response.read().decode('utf-8'))
                for path in ['/static/finance.js','/static/finance.css']:
                    with urlopen(address + path, timeout=5) as response:
                        self.assertEqual(response.status, 200)
            finally:
                server.should_exit = True
                thread.join(timeout=10)


if __name__ == '__main__':
    unittest.main()
