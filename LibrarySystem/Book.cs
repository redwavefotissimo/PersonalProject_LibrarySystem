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
    public partial class Book : Form
    {
        private BookManager bookmanager;
        private int book_id;
        private string kind;
        private string type;
        private string message;

        public Book()
        {
            InitializeComponent();
        }

        public void setupfield(int book_id, string title, string author, string description, int edition, int num_of_copy, string kind, string type)
        {
            this.book_id = book_id;
            this.kind = kind;
            this.type = type;
            textBox1.Text = title;
            textBox2.Text = author;
            textBox3.Text = description;
            textBox4.Text = edition.ToString();
            textBox5.Text = num_of_copy.ToString();
        }

        private void Book_Load(object sender, EventArgs e)
        {
            bookmanager = new BookManager();
            comboBox1.DataSource = Enum.GetNames(typeof(List.Kind));
            comboBox2.DataSource = Enum.GetNames(typeof(List.FictionType));
            comboBox2.Text = kind;
            comboBox2.Text = type;
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

        private void button3_Click(object sender, EventArgs e)
        {
            // close
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // update
            if (validatefield() == true)
            {
                bookmanager.Update(new object[]{textBox1.Text,
                                            textBox2.Text,
                                            textBox4.Text,
                                            textBox3.Text,
                                            comboBox2.Text,
                                            comboBox1.Text,
                                            textBox5.Text
                                           },
                                   new object[]{book_id
                                           });
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // insert
            if (validatefield() == true)
            {
                bookmanager.Insert(new object[]{textBox1.Text,
                                            textBox2.Text,
                                            textBox4.Text,
                                            textBox3.Text,
                                            comboBox2.Text,
                                            comboBox1.Text,
                                            textBox5.Text
                                           });
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
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
            if (textBox4.Text == "") // Edtion
            {
                message += "Edition is Required!" + Environment.NewLine;
            }
            if (textBox5.Text == "") // Number of Copy
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
    }
}
