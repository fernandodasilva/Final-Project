using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Database
{

    public class Item
    {
        public int _SystemNumber;
        public int _CallNumber;
        public string _Title;
        public string _Author;
        public string _Subject;

        public string _Status;




        public Item(int systemNumber, int callNumber, string title, string author, string subject)
        {
            _SystemNumber = systemNumber;
            _CallNumber = callNumber;
            _Title = title;
            _Author = author;
            _Subject = subject;
        }

        public Item(int systemNumber, int callNumber, string title, string author, string subject, string status)
        {
            _SystemNumber = systemNumber;
            _CallNumber = callNumber;
            _Title = title;
            _Author = author;
            _Subject = subject;
            _Status = status;
        }

        public static Item GetTestItem()
        {
            return new Item(0, 0, "Test Book", "Test Author", "Test Subject");
        }
    }
}
