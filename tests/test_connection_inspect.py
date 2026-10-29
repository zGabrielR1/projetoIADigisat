"""Connection inspection uses mocked Mongo clients and never alters local settings."""
import unittest
from unittest.mock import MagicMock, patch
from fastapi import HTTPException
from pymongo.errors import OperationFailure
from insight_connection import ConnectionSettings, inspect_connection, test_connection as run_check


class ConnectionInspectionTests(unittest.TestCase):
    def setUp(self):
        self.settings = ConnectionSettings(database='Test', username='reader', password='private-test-value', require_read_only=True)
        self.client = MagicMock()
        self.client.__enter__.return_value = self.client
        self.client.__getitem__.return_value.list_collection_names.return_value = ['Movimentacoes']
        self.client.admin.command.return_value = {'authInfo': {'authenticatedUsers': [{}],
            'authenticatedUserPrivileges': [{'actions': ['find', 'listCollections']}]}}

    def inspect(self):
        with patch('pymongo.MongoClient', return_value=self.client):
            result = inspect_connection(self.settings)
        self.assertNotIn(self.settings.password, str(result))
        return result

    def test_read_account_completes_all_checks(self):
        result = self.inspect()
        self.assertTrue(result['ready'])
        self.assertEqual([x['status'] for x in result['checks']], ['ok']*4)

    def test_strict_privileges_block_broad_account_and_save_contract(self):
        self.client.admin.command.return_value['authInfo']['authenticatedUserPrivileges'][0]['actions'].append('insert')
        result = self.inspect()
        self.assertFalse(result['ready'])
        self.assertEqual(result['checks'][-1]['status'], 'attention')
        with patch('pymongo.MongoClient', return_value=self.client):
            with self.assertRaises(HTTPException): run_check(self.settings)

    def test_auth_failure_does_not_expose_server_exception(self):
        self.client.admin.command.side_effect = OperationFailure(self.settings.password, code=18)
        result = self.inspect()
        self.assertFalse(result['ready'])
        self.assertEqual([x['status'] for x in result['checks']], ['ok','failed','skipped','skipped'])
        self.assertIn('Autenticação recusada', result['message'])

    def test_database_access_denied_is_separate_from_login(self):
        self.client.__getitem__.return_value.list_collection_names.side_effect = OperationFailure('private', code=13)
        result = self.inspect()
        self.assertEqual([x['status'] for x in result['checks']], ['ok','ok','failed','skipped'])
        self.assertIn('role read', result['message'])

    def test_missing_password_file_stops_before_network(self):
        self.settings.password = None
        with patch('insight_connection.password_from_current', side_effect=HTTPException(503, 'Arquivo não encontrado')), patch('pymongo.MongoClient') as client:
            result = inspect_connection(self.settings)
        client.assert_not_called()
        self.assertEqual([x['status'] for x in result['checks']], ['failed','skipped','skipped','skipped'])

    def test_optional_privileges_still_show_attention(self):
        self.settings.require_read_only = False
        self.client.admin.command.return_value = {}
        result = self.inspect()
        self.assertTrue(result['ready'])
        self.assertEqual(result['checks'][-1]['status'], 'attention')
