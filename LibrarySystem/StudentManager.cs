using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace LibrarySystem
{
    class StudentManager: LibraryManager
    {
        public override void Insert(object[] paramval)
        {
            try
            {
                base.Insert(null);
                cmd.CommandText = "Insert Into student (First_Name, Middle_Name, Last_Name, Course, Year) Values " +
                                   "(@first_name, @middle_name, @last_name, @course, @year)";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@first_name", paramval[0].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@middle_name", paramval[1].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@last_name", paramval[2].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@year", int.Parse(paramval[3].ToString())));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@course", paramval[4].ToString()));

                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                this.message = e.Message;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        
        public override void Update(object[] paramsetval, object[] paramwherval)
        {
            try
            {
                base.Update(null, null);
                cmd.CommandText = "Update student Set  Where Student_id=@student_id";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@student_id", int.Parse(paramwherval[0].ToString())));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@first_name", paramsetval[0].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@middle_name", paramsetval[1].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@last_name", paramsetval[2].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@year", int.Parse(paramsetval[3].ToString())));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@course", paramsetval[4].ToString()));

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                this.message = e.Message;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public override void Select(object[] paramwhereval)
        {
            string query = ""; // store the constructed query here

            // This for loop will construct the query for where clause
            // the paramwhereval[i].tostring() value is ColumnName:Value ...... [0]:[1] after split
            for (int i = 0; i < paramwhereval.Length; i++)
            {
                if (paramwhereval[i] != null)
                {
                    if (i == 0 || i == 1 || i == 2)
                        query += " and " + paramwhereval[i].ToString().Split(':')[0] + " LIKE @" + paramwhereval[i].ToString().Split(':')[0].ToLower();
                    else
                        query += " and " + paramwhereval[i].ToString().Split(':')[0] + "=@" + paramwhereval[i].ToString().Split(':')[0].ToLower();
                }
            }

            try
            {
                base.Select(null);
                cmd.CommandText = "Select Student_id, CONCAT_WS(' ', First_Name , Middle_Name , Last_Name) as Name, Course, Year From student Where true" + query;

                // This for loop is for constructing the mysql parameter 
                // the paramwhereval[i].tostring() value is ColumnName:Value ...... [0]:[1] after split
                for (int i = 0; i < paramwhereval.Length; i++)
                {
                    if (paramwhereval[i] != null)
                    {
                        if (i == 0 || i == 1 || i == 2)
                            cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@" + paramwhereval[i].ToString().Split(':')[0].ToLower(), "%" + paramwhereval[i].ToString().Split(':')[1] + "%"));
                        else
                            cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@" + paramwhereval[i].ToString().Split(':')[0].ToLower(), paramwhereval[i].ToString().Split(':')[1]));
                    }
                }

                rdr = cmd.ExecuteReader();
                dt = new System.Data.DataTable();
                dt.Load(rdr);
            }
            catch (Exception e)
            {
                this.message = e.Message;
            }
            finally
            {
                if (rdr.IsClosed == false)
                {
                    rdr.Close();
                }
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public override void Delete(object[] paramwhereval)
        {
            try
            {
                base.Delete(null);
                cmd.CommandText = "Delete From student Where Student_id=@student_id";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@student_id", int.Parse(paramwhereval[0].ToString())));

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                this.message = e.Message;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public void ListOfStudentWhoBorrowedThisBook(int book_id)
        {
            try
            {
                connectdb();
                cmd.CommandText = "Select Student_id, CONCAT_WS(' ', First_Name , Middle_Name , Last_Name) as Name From student where Student_id in (Select Student_id From borrowed Where Book_id=@book_id and Date_Returned is null)";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@book_id", book_id));

                rdr = cmd.ExecuteReader();
                dt = new System.Data.DataTable();
                dt.Load(rdr);
            }
            catch (Exception e)
            {
                this.message = e.Message;
            }
            finally
            {
                if (rdr.IsClosed == false)
                {
                    rdr.Close();
                }
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}
