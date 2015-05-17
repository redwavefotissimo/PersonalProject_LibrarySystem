using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystem
{
    class BookManager: LibraryManager
    {
        public override void Insert(object[] paramval)
        {
            try
            {
                base.Insert(null);
                cmd.CommandText = "Insert Into book (Title, Author, Edition, Description, Type, Kind, Num_Of_Copy) Values " + 
                                    "(@title, @author, @edition, @description, @type, @kind, @num_of_copy)";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@title", paramval[0].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@author", paramval[1].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@edition", int.Parse(paramval[2].ToString())));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@description", paramval[3].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@type", paramval[4].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@kind", paramval[5].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@num_of_copy", int.Parse(paramval[6].ToString())));

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
                cmd.CommandText = "Update book Set Title=@title, Author=@author, description=@description, Edition=@edition, Kind=@kind, Type=@type, Num_Of_Copy=@num_of_copy Where Book_id=@book_id";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@book_id", int.Parse(paramwherval[0].ToString())));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@title", paramsetval[0].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@author", paramsetval[1].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@edition", int.Parse(paramsetval[2].ToString())));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@description", paramsetval[3].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@type", paramsetval[4].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@kind", paramsetval[5].ToString()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@num_of_copy", int.Parse(paramsetval[6].ToString())));               

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
            string num_of_copy_query = ""; // store the constructed query here for # of copy

            // This for loop will construct the query for where clause
            // the paramwhereval[i].tostring() value is ColumnName:Value ...... [0]:[1] after split
            for (int i = 0; i < paramwhereval.Length; i++)
            {
                if (paramwhereval[i] != null)
                {
                    if(i == 0 || i == 1 || i == 2)
                        query += " and " + paramwhereval[i].ToString().Split(':')[0] + " LIKE @" + paramwhereval[i].ToString().Split(':')[0].ToLower();
                    else
                        query += " and " + paramwhereval[i].ToString().Split(':')[0] + "=@" + paramwhereval[i].ToString().Split(':')[0].ToLower();
                }
            }

            // intentionally created the paramwhereval.length as 6, to know that the grid is looking for remaining availabe of the particular book to be borrowed
            if (paramwhereval.Length == 6)
            {
                num_of_copy_query = "(Num_Of_Copy - (select count(*) from borrowed as br where br.Book_id=bk.Book_id and Date_Returned is null)) as Copies_available";
            }
            else // original and correct paramwhereval.length is 5, to know that the grid is looking for the total of the particular book the library has
            {
                num_of_copy_query = "Num_Of_Copy";
            }

            try
            {
                base.Select(null);
                cmd.CommandText = "Select Book_id, Title, Author, Description, Edition, Kind, Type, " +  num_of_copy_query + " From book as bk Where true" + query;

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
                cmd.CommandText = "Delete From book Where Book_id=@book_id";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@book_id", int.Parse(paramwhereval[0].ToString())));

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

        public void ListOfBooksNotYetReturnedByThisStudent(int student_id)
        {
            try
            {
                connectdb();
                cmd.CommandText = "Select Book_id, Title From book Where Book_id in (Select Book_id from borrowed where Student_id=@student_id and Date_Returned is null)";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@student_id", student_id));

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
