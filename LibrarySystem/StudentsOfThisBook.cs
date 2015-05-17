using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class StudentsOfThisBook : Form
    {

        private Label[] studentName;
        private Button[] returnButton;
        private StudentManager studentmanager;
        private BorrowedManager borrowedmanager;
        private int book_id;
        private Font font;
        private DataTable dt;

        public StudentsOfThisBook()
        {
            InitializeComponent();
        }

        public void setupfield(int book_id, string title)
        {
            this.book_id = book_id;
            label1.Text = title;
        }

        // setup the labels and button
        private void init()
        {
            try
            {
                studentmanager.ListOfStudentWhoBorrowedThisBook(book_id);
            }
            catch
            {
                MessageBox.Show(studentmanager.message);
            }
            dt = studentmanager.getDt();
            font = new Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            studentName = new Label[studentmanager.getTotalRowRetrieved()];
            returnButton = new Button[studentmanager.getTotalRowRetrieved()];

            for (int i = 0; i < studentmanager.getTotalRowRetrieved(); i++)
            {
                // Labels
                studentName[i] = new Label();
                studentName[i].AutoSize = true;
                studentName[i].Location = new Point(42, 20 + ((i * 30) + 8));
                studentName[i].Text = dt.Rows[i].ItemArray[1].ToString();
                this.Controls.Add(studentName[i]);

                // Buttons
                returnButton[i] = new Button();
                returnButton[i].AutoSize = true;
                returnButton[i].Location = new Point(227, 20 + ((i * 30) + 5));
                returnButton[i].Text = "Return";
                returnButton[i].Tag = int.Parse(dt.Rows[i].ItemArray[0].ToString());
                returnButton[i].Click += new EventHandler(StudentsOfThisBook_Click);
                this.Controls.Add(returnButton[i]);
            }

            // for dynamic growth in heigth, formula is form size + (button size * total # of buttons)
            this.Size = new Size(320, 64 + (returnButton.Length * 30));
        }

        void StudentsOfThisBook_Click(object sender, EventArgs e)
        {
            // convert the sender object into button, without converting into button you can only get the text representaion of that button being fired
            Button btn = (Button)sender;
            borrowedmanager.Update(null, new object[]{book_id,
                                                      btn.Tag
                                                     });
            // pop up message if there is a fine
            int fine = borrowedmanager.FineOfThisStudent(book_id, int.Parse(btn.Tag.ToString()));
            if (fine > 0)
            {
                MessageBox.Show("Please Pay " + fine, "Warning! Book Borrowed Exceeded");
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void StudentsOfThisBook_Load(object sender, EventArgs e)
        {
            studentmanager = new StudentManager();
            borrowedmanager = new BorrowedManager();
            init();
        }

        private void StudentsOfThisBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

    }
}
