using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Database
{
    public class BooksDB : SqliteHelper
    {

        private const String Tag = "Riz: LocationDb:\t";


        private const String TABLE_NAME = "Books";
        private const String KEY_SYSTEMNUMBER = "systemNumber";
        private const String KEY_CALLNUMBER = "callNumber";
        private const String KEY_TITLE = "title";
        private const String KEY_AUTHOR = "author";
        private const String KEY_SUBJECT = "subject";
        private const String KEY_STATUS = "status";
        private String[] COLUMNS = new String[] {KEY_SYSTEMNUMBER,
                                                KEY_CALLNUMBER,
                                                KEY_TITLE,
                                                KEY_AUTHOR,
                                                KEY_SUBJECT,
                                                KEY_STATUS};


        public BooksDB() : base()
        {
            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME +
                " ( " +
                KEY_SYSTEMNUMBER + "TEXT PRIMARY KEY " +
                KEY_CALLNUMBER + "TEXT, " +
                KEY_TITLE + "TEXT, " +
                KEY_AUTHOR + "TEXT " +
                KEY_SUBJECT + "TEXT " +
                KEY_STATUS + "TEXT )";

            dbcmd.ExecuteNonQuery();
        }

        public void AddData(Item book)
        {
            IDbCommand dbcmd = GetDbCommand();

            dbcmd.CommandText = 
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_SYSTEMNUMBER + ", "
                + KEY_CALLNUMBER + ", "
                + KEY_TITLE + ", "
                + KEY_AUTHOR + ", "
                + KEY_SUBJECT + ", "
                + KEY_STATUS + " ) "

                + "VALUES ( '"
                + book._SystemNumber + "', '"
                + book._CallNumber + "', '"
                + book._Title + "', '"
                + book._Author + "', '"
                + book._Subject + "', '"
                + book._Status + "' )";
            dbcmd.ExecuteNonQuery();
        }


        public override IDataReader GetDataByID(int id)
        {
            return base.GetDataByID(id);
        }


        public override IDataReader GetDataByString(string str)
        {
            Debug.Log(Tag + "Getting Location: " + str);
            IDbCommand dbcmd = GetDbCommand();

            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_SYSTEMNUMBER + " = '" + str + "'";

            return dbcmd.ExecuteReader();
        }

        public override void DeleteDataByString(string str)
        {
            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText =
                "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_SYSTEMNUMBER + " = '" + str + "'";
            dbcmd.ExecuteNonQuery();
        }

        public override void DeleteDataById(int id)
        {
            Debug.Log(Tag + "Deleting Location: " + id);
            base.DeleteDataById(id);
        }

        public override void DeleteAllData()
        {
            Debug.Log(Tag + "Deleting Table");
            base.DeleteAllData();
        }

        public override IDataReader GetAllData()
        {
            return base.GetAllData(TABLE_NAME);
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

