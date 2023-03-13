using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUi
{
    public static class Database
    {
        public static void ExecuteMYSQLQuery(MySql.Data.MySqlClient.MySqlConnection connection, string Query)
        {
            string connectionString;
            try
            {
                connectionString = Encryption.Decrypt(CredentialUtil.GetPassword(Encryption.Encrypt(Environment.MachineName + "MYSQL_ConnectionString")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            MySql.Data.MySqlClient.MySqlCommand updatefinal = new MySql.Data.MySqlClient.MySqlCommand(Query, connection);
            updatefinal.ExecuteReader();
        }
        public static DataTable GetMYSQLData(MySql.Data.MySqlClient.MySqlConnection connection, string Query)
        {
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(Query, connection);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return (dataTable);
        }
        public static DataTable GetSQLData(MySql.Data.MySqlClient.MySqlConnection connection, string Query)
        {
            var dataTable = new DataTable();
            return (dataTable);
        }
        public static void ExecuteSQLQuery(MySql.Data.MySqlClient.MySqlConnection connection, string Query)
        {

        }
    }
}
