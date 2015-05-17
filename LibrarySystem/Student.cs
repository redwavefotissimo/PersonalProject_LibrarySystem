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
    public partial class Student : Form
    {
        private StudentManager studentmanager;
        private int student_id;
        private string course;
        private string message;

        public Student()
        {
            InitializeComponent();
        }

        public void setupfield(int student_id, string name, string course, int year)
        {
            this.student_id = student_id;
            this.course = course;
            if (name.Split(' ').Length == 1)
            {
                textBox1.Text = name.Split(' ')[0];
            }
            else if (name.Split(' ').Length == 2)
            {
                textBox1.Text = name.Split(' ')[0];
                textBox3.Text = name.Split(' ')[1];
            }
            else if (name.Split(' ').Length == 3)
            {
                textBox1.Text = name.Split(' ')[0];
                textBox2.Text = name.Split(' ')[1];
                textBox3.Text = name.Split(' ')[2];
            }
            textBox4.Text = year.ToString();
        }

        private void Student_Load(object sender, EventArgs e)
        {
            studentmanager = new StudentManager();
            comboBox1.DataSource = List.Course;
            comboBox1.Text = course;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // close
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // insert
            if (validatefield() == true)
            {
                studentmanager.Insert(new object[] {textBox1.Text,
                                                textBox2.Text,
                                                textBox3.Text,
                                                textBox4.Text,
                                                comboBox1.Text
                                               });
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // update
            if (validatefield() == true)
            {
                studentmanager.Update(new object[]{textBox1.Text,
                                               textBox2.Text,
                                               textBox3.Text,
                                               textBox4.Text,
                                               comboBox1.Text
                                              },
                                      new object[]{student_id
                                              });
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool validatefield()
        {
            if (textBox1.Text == "") // First Name
            {
                message += "First Name is Required!" + Environment.NewLine;
            }
            if (textBox3.Text == "") // Last Name
            {
                message += "Last Name is Required!" + Environment.NewLine;
            }
            if (textBox4.Text == "") // Year
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
    }
}
