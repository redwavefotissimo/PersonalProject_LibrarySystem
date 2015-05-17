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
    public partial class AdminBook : Form
    {

        private BookManager bookmanager;
        private Book book;
        private string message;
        private int maxpage, maxrowdisplayed = 6;
        private DataTable dt;

        public AdminBook()
        {
            InitializeComponent();
        }

        private void AdminBook_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Enum.GetNames(typeof(List.Kind));
            comboBox2.DataSource = Enum.GetNames(typeof(List.FictionType));
            bookmanager = new BookManager();
            message = "";
            constructSelectQuery();
            // hide unwanted columns
            dataGridView1.Columns[2].Visible = false;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (validatefield() == true)
            {
                bookmanager.Insert(new object[]{
                                    textBox1.Text,
                                    textBox2.Text,
                                    textBox5.Text,
                                    textBox3.Text,
                                    comboBox2.Text,
                                    comboBox1.Text,
                                    textBox6.Text
                                    });
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
                    book = new Book();
                    book.setupfield(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()),
                                    dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(),
                                    dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(),
                                    dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString(),
                                    int.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString()),
                                    int.Parse(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString()),
                                    dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString(),
                                    dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString()
                                   );
                    book.ShowDialog();
                    if (book.DialogResult == DialogResult.OK)
                    {
                        constructSelectQuery();
                    }
                    break;
                case 1: // delete button
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this Book?", "Warning!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        bookmanager.Delete(new object[] { dataGridView1.Rows[e.RowIndex].Cells[2].Value });
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
            if (textBox1.Text == "") // Title
            {
                message += "Title is Required!" + Environment.NewLine;
            }
            if (textBox2.Text == "") // Author
            {
                message += "Author is Required!" + Environment.NewLine;
            }
            if (textBox3.Text == "") // Description
            {
                message += "Description is Required!" + Environment.NewLine;
            }
            if (textBox5.Text == "") // Edtion
            {
                message += "Edition is Required!" + Environment.NewLine;
            }
            if (textBox6.Text == "") // Number of Copy
            {
                message += "# of Copy is Required!" + Environment.NewLine;
            }
            if (comboBox1.Text == "Both") // Kind
            {
                message += "Kind must not be Both!" + Environment.NewLine;
            }
            if (comboBox2.Text == "All") // Type
            {
                message += "Type must not be All!" + Environment.NewLine;
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
