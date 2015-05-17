using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystem
{
    class BorrowedManager: LibraryManager
    {
        private SettingManager sm;

        public override void Insert(object[] paramval) // borrow function
        {
            sm = new SettingManager();

            try
            {
                base.Insert(null);
                cmd.CommandText = "Insert Into borrowed (Date_Borrowed, Date_Due, Book_id, Student_id) Values (Now(), ADDDATE(Now(), interval @daysallowed day), @book_id, @student_id)";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@daysallowed", sm.daysallowed()));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@book_id", int.Parse(paramval[0].ToString())));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@student_id", int.Parse(paramval[1].ToString())));

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

        public override void Update(object[] paramsetval, object[] paramwherval) // return function
        {
            try
            {
                base.Update(null, null);
                cmd.CommandText = "Update borrowed set Date_Returned=Now() Where Book_id=@book_id and Student_id=@student_id";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@book_id", int.Parse(paramwherval[0].ToString())));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@student_id", int.Parse(paramwherval[1].ToString())));

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

        public int FineOfThisStudent(int book_id, int student_id)
        {
            int fine = 0;
            sm = new SettingManager();

            try
            {
                connectdb();
                cmd.CommandText = "SELECT (Datediff(Date_Returned, Date_Borrowed) - Datediff(Date_Due, Date_Borrowed)) as Excess_Days FROM library.borrowed where Date_returned is not null and Book_id=@book_id and Student_id=@student_id";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@book_id", book_id));
                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@student_id", student_id));

                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (rdr.HasRows == true)
                    {
                        fine = sm.fine() * int.Parse(rdr.GetValue(rdr.GetOrdinal("Excess_Days")).ToString());
                    }
                }
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
            return fine;
        }

        public bool totalBooksCurrentlyBorrowedByThisStudentHasReachedLimit(int student_id)
        {
            bool result = false;
            sm = new SettingManager();

            try
            {
                connectdb();
                cmd.CommandText = "SELECT count(Book_id) as total from borrowed where Student_id=@student_id and Date_Returned is null";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@student_id", student_id));

                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (rdr.HasRows == true)
                    {
                        if(int.Parse(rdr.GetValue(rdr.GetOrdinal("total")).ToString()) > sm.maxbookallowed())
                        {
                            this.message = "";
                            result = true;
                        }
                    }
                }
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
            return result;
        }
    }
}
