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
    public partial class Borrow : Form
    {
        private StudentManager studentmanager;
        private BorrowedManager borrowmanager;
        private int book_id;

        public Borrow()
        {
            InitializeComponent();
        }

        public void setupfield(int book_id, string book_title)
        {
            this.book_id = book_id;
            textBox1.Text = book_title;
        }

        private void Borrow_Load(object sender, EventArgs e)
        {
            studentmanager = new StudentManager();
            borrowmanager = new BorrowedManager();
            studentmanager.Select(new object[0]);
            comboBox1.DataSource = studentmanager.getDt();
            comboBox1.ValueMember = "Student_id";
            comboBox1.DisplayMember = "Name";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // borrow, first check if that person had reached max book allowed to borrow
            if (borrowmanager.totalBooksCurrentlyBorrowedByThisStudentHasReachedLimit(int.Parse(comboBox1.SelectedValue.ToString())) == false)
            {
                borrowmanager.Insert(new object[]{book_id,
                                              comboBox1.SelectedValue  
                                             });
            }
            else
            {
                MessageBox.Show(borrowmanager.message);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // close
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
