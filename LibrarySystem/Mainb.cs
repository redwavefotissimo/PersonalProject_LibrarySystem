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
    public partial class Mainb : Form
    {
        private Login lg;
        private Setting st;
        private AdminBook ab;
        private Mains ms;
        private Report rpt;
        private BookManager bookmanager;
        private Borrow borrow;
        private StudentsOfThisBook studentofthisbook;
        private int maxpage, maxrowdisplayed = 7;
        private DataTable dt;

        public Mainb()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lg.ShowDialog();
            if (lg.DialogResult == DialogResult.OK)
            {
                ab = new AdminBook();
                ab.Show();
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
            ms = new Mains();
            ms.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rpt = new Report();
            rpt.Show();
        }

        private void Mainb_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Enum.GetNames(typeof(List.Kind));
            comboBox2.DataSource = Enum.GetNames(typeof(List.FictionType));
            lg = new Login();
            bookmanager = new BookManager();
            constructSelectQuery();
            // hide unwanted columns
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Both")
            {
                comboBox2.Enabled = false;
            }
            else if (comboBox1.Text == "Fiction")
            {
                comboBox2.Enabled = true;
                comboBox2.DataSource = Enum.GetNames(typeof(List.FictionType));
            }
            else
            {
                comboBox2.Enabled = true;
                comboBox2.DataSource = Enum.GetNames(typeof(List.NonFictionType));
            }
            constructSelectQuery();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            constructSelectQuery();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            constructSelectQuery();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            constructSelectQuery();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            constructSelectQuery();
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) // borrow
            {
                if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString()) == 0) // the particular book is not available anymore
                {
                    MessageBox.Show("The book is not available anymore", "Warning!");
                }
                else
                {
                    borrow = new Borrow();
                    borrow.setupfield(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()),
                                        dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    borrow.ShowDialog();
                    if (borrow.DialogResult == DialogResult.OK)
                    {
                        constructSelectQuery();
                    }
                }
            }
            else // other column like title, author, etc
            { 
                // for clicking retrieve on each student listed who borrowed the particular book
                studentofthisbook = new StudentsOfThisBook();
                studentofthisbook.setupfield(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()),
                                             dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                studentofthisbook.ShowDialog();
                if (studentofthisbook.DialogResult == DialogResult.OK)
                {
                    constructSelectQuery();
                }
            }
        }

        private void constructSelectQuery() // much like refresh, gets all the values in textbox and combobox and store it to object array
        {
            object[] query = new object[6];

            // title
            if (textBox1.Text != "")
            {
                query[0] = "Title:" + textBox1.Text;
            }
            // author
            if (textBox2.Text != "")
            {
                query[1] = "Author:" + textBox2.Text;
            }
            // description
            if (textBox3.Text != "")
            {
                query[2] = "Description:" + textBox3.Text;
            }
            // kind
            if (comboBox1.Text != "Both")
            {
                query[3] = "kind:" + comboBox1.Text;
            }
            // type
            if (comboBox2.Text != "All")
            {
                query[4] = "Type:" + comboBox2.Text;
            }

            if (bookmanager != null)
            {
                bookmanager.Select(query);
                // place the datatable retrieve from database to temp datatable dt for use with LINQ
                dt = bookmanager.getDt();
                // load the temp dt to grid with limited rows via LINQ
                loadDataTableToGrid();
                // get max page
                maxpage = bookmanager.maxpage(maxrowdisplayed);
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
