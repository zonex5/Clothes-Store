using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ClothesStore
{
    class SQL
    {
        static public SQLiteConnection connection;

        static public SQLiteConnection Connection
        {
            get
            { 
                if (connection == null)
                    connection = new SQLiteConnection(string.Format("Data Source={0};", "parts.db"));
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                return connection;
            }
        }

        static public void ExecSQL(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, Connection);
            command.ExecuteNonQuery();
        }

        static public void ResetConnection()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
                connection.Close();
            connection = null;
        }
    }
}
