using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibrarySystem
{
    class LibraryManager
    {
        protected MySqlConnection con;
        protected MySqlCommand cmd;
        protected MySqlDataReader rdr;
        protected DataTable dt;
        public string message = "";
        private int totalRow;

        protected void connectdb()
        {
            con = new MySqlConnection("Server=127.0.0.1;Database=library;Uid=root;Pwd=root;");
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
        }

        public virtual void Insert(object[] paramval)
        {
            connectdb();
        }

        public virtual void Update(object[] paramsetval, object[] paramwherval)
        {
            connectdb();
        }

        public virtual void Select(object[] paramwhereval)
        {
            connectdb();
        }

        public virtual void Delete(object[] paramwhereval)
        {
            connectdb();
        }

        public DataTable getDt()
        {
            totalRow = this.dt.Rows.Count;
            return this.dt;
        }

        public int getTotalRowRetrieved()
        {
            return totalRow;
        }

        public int maxpage(int n)
        {
            int maxpage = totalRow / n;
            if (((double)totalRow / (double)n).ToString().Contains(".") == true)
            {
                maxpage += 1;
            }
            return maxpage;
        }

    }
}
