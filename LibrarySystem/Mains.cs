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
    public partial class Mains : Form
    {
        private Login lg;
        private Setting st;
        private AdminStudent ads;
        private Report rpt;
        private StudentManager studentmanager;
        private BooksOfThisStudent bookofthisstudent;
        private int maxpage, maxrowdisplayed = 8;
        private DataTable dt;

        public Mains()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lg.ShowDialog();
            if (lg.DialogResult == DialogResult.OK)
            {
                ads = new AdminStudent();
                ads.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            lg.ShowDialog();
            if (lg.DialogResult == DialogResult.OK)
            {
                st = new Setting();
                st.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Mainb.ActiveForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rpt = new Report();
            rpt.Show();
        }

        private void Mains_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = List.Course;
            lg = new Login();
            studentmanager = new StudentManager();
            constructSelectQuery();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            constructSelectQuery();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            constructSelectQuery();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            constructSelectQuery();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // for clicking retrieve on each student listed who borrowed the particular book
            bookofthisstudent = new BooksOfThisStudent();
            bookofthisstudent.setupfield(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()),
                                         dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            bookofthisstudent.ShowDialog();
            if (bookofthisstudent.DialogResult == DialogResult.OK)
            {
                constructSelectQuery();
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
