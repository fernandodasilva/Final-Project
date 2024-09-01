using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Data;

namespace Database
{
    public class SqliteHelper
    {

        private const string Tag = "Riz: SqliteHelper:\t";

        private const string database_name = "my_db";

        public string db_connection_string;
        public IDbConnection db_connection;

        public SqliteHelper()
        {
            db_connection_string = "URI=file" + Application.persistentDataPath + "/" + database_name;
            Debug.Log("db_connection_string " + db_connection_string);
            db_connection = new SqliteConnection(db_connection_string);
            db_connection.Open();
        }

        ~SqliteHelper()
        {
            db_connection.Close();
        }

        //FUNÇÕES VIRTUAIS

        public virtual IDataReader GetDataByID(int id)
        {
            throw null;
        }

        public virtual IDataReader GetDataByString(string str)
        {
            throw null;
        }

        public virtual void DeleteDataById(int id)
        {
            throw null;
        }

        public virtual void DeleteDataByString(string str)
        {
            throw null;
        }

        public virtual IDataReader GetAllData()
        {
            throw null;
        }

        public virtual void DeleteAllData()
        {
            throw null;
        }

        public virtual IDataReader GetNumberOfRows()
        {
            throw null;
        }

        //HELPER FUNCTIONS

        public IDbCommand GetDbCommand()
        {
            return db_connection.CreateCommand();
        }

        public IDataReader GetAllData(string tableName)
        {
            IDbCommand dbcmd = db_connection.CreateCommand();
            dbcmd.CommandText = "SELECT * FROM " + tableName;

            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public void DeleteAllData(string tableName)
        {
            IDbCommand dbcmd = db_connection.CreateCommand();
            dbcmd.CommandText = "DROP TABLE IF EXISTS " + tableName;
            dbcmd.ExecuteNonQuery();
        }


        public IDataReader GetNumberOfRows(string tableName)
        {
            IDbCommand dbcmd = db_connection.CreateCommand();
            dbcmd.CommandText = "SELECT COALESCE(MAX(id)+1, 0) FROM " + tableName;
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public void Close()
        {
            db_connection.Close();
        }
    }
}