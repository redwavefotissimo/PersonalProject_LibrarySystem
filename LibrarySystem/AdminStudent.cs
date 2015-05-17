using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class AdminStudent : Form
    {
        private StudentManager studentmanager;
        private Student student;
        private string message;
        private int maxpage, maxrowdisplayed = 9;
        private DataTable dt;

        public AdminStudent()
        {
            InitializeComponent();
        }

        private void AdminStudent_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = List.Course;
            studentmanager = new StudentManager();
            message = "";
            constructSelectQuery();
            // hide unwanted columns
            dataGridView1.Columns[2].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object[] field = new object[5];
            if (validatefield() == true)
            {               
                if (textBox1.Text.Split(' ').Length == 1)
                {
                    field[0] = textBox1.Text.Split(' ')[0];
                    field[1] = field[2] = "";
                }
                else if (textBox1.Text.Split(' ').Length == 2)
                {
                    field[0] = textBox1.Text.Split(' ')[0];
                    field[1] = "";
                    field[2] = textBox1.Text.Split(' ')[1];
                }
                else if (textBox1.Text.Split(' ').Length == 3)
                {
                    field[0] = textBox1.Text.Split(' ')[0];
                    field[1] = textBox1.Text.Split(' ')[1];
                    field[2] = textBox1.Text.Split(' ')[2];
                }
                field[3] = textBox2.Text;
                field[4] = comboBox1.Text;               

                studentmanager.Insert(field);
                constructSelectQuery();
            }
            else
            {
                MessageBox.Show(message, "Error");
                message = "";
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex) 
            {
                case 0: // edit button
                    student = new Student();
                    student.setupfield(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()),
                                        dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(),
                                        dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(),
                                        int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString())
                                      );
                    student.ShowDialog();
                    if (student.DialogResult == DialogResult.OK)
                    {
                        constructSelectQuery();
                    }
                    break;
                case 1: // delete button
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this User?", "Warning!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        studentmanager.Delete(new object[] { dataGridView1.Rows[e.RowIndex].Cells[2].Value });
                        constructSelectQuery();
                    }
                    break;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                if (int.Parse(textBox4.Text) > maxpage)
                {
                    textBox4.Text = maxpage.ToString();
                }
                loadDataTableToGrid();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Text = (int.Parse(textBox4.Text) - 1).ToString();
            if (int.Parse(textBox4.Text) <= 1)
            {
                button3.Enabled = false;
                button6.Enabled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox4.Text = (int.Parse(textBox4.Text) + 1).ToString();
            if (int.Parse(textBox4.Text) >= maxpage)
            {
                button3.Enabled = true;
                button6.Enabled = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox4.Text = maxpage.ToString();
            button3.Enabled = true;
            button6.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Text = "1";
            button3.Enabled = false;
            button6.Enabled = true;
        }

        private bool validatefield()
        {
            if (textBox1.Text == "") // Name
            {
                message += "Name is Required!" + Environment.NewLine;
            }
            if (textBox2.Text == "") // Year
            {
                message += "Year is Required!" + Environment.NewLine;
            }
            if (comboBox1.Text == "All") // Course
            {
                message += "Course must not be All!" + Environment.NewLine;
            }

            if (message != "")
            {
                return false;
            }
            return true;
        }

        private void constructSelectQuery() // much like refresh, gets all the values in textbox and combobox and store it to object array
        {
            object[] query = new object[5];

            // Name
            if (textBox1.Text != "")
            {
                if (textBox1.Text.Split(' ').Length == 1)
                {
                    query[0] = "First_Name:" + textBox1.Text.Split(' ')[0];
                }
                else if (textBox1.Text.Split(' ').Length == 2)
                {
                    query[0] = "First_Name:" + textBox1.Text.Split(' ')[0];
                    query[1] = "Last_Name:" + textBox1.Text.Split(' ')[1];
                }
                else if (textBox1.Text.Split(' ').Length == 3)
                {
                    query[0] = "First_Name:" + textBox1.Text.Split(' ')[0];
                    query[1] = "Middle_Name:" + textBox1.Text.Split(' ')[1];
                    query[2] = "Last_Name:" + textBox1.Text.Split(' ')[2];
                }
            }
            // Year
            if (textBox2.Text != "")
            {
                query[3] = "Year:" + textBox2.Text;
            }
            // Course
            if (comboBox1.Text != "All")
            {
                query[4] = "Course:" + comboBox1.Text;
            }


            if (studentmanager != null)
            {
                studentmanager.Select(query);
                // place the datatable retrieve from database to temp datatable dt for use with LINQ
                dt = studentmanager.getDt();
                // load the temp dt to grid with limited rows via LINQ
                loadDataTableToGrid();
                // get max page
                maxpage = studentmanager.maxpage(maxrowdisplayed);
                // set max page
                label7.Text = "/ " + maxpage.ToString();
            }
        }

        private void loadDataTableToGrid()
        {
            try
            {
                var result = (from datatable
                              in dt.AsEnumerable()
                              select datatable).Skip(maxrowdisplayed * (int.Parse(textBox4.Text) - 1)).Take(maxrowdisplayed);

                dataGridView1.DataSource = result.CopyToDataTable();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
