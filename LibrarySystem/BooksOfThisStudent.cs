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
    public partial class BooksOfThisStudent : Form
    {
        private Label[] bookTitle;
        private Button[] returnButton;
        private BookManager bookmanager;
        private BorrowedManager borrowedmanager;
        private int student_id;
        private Font font;
        private DataTable dt;

        public BooksOfThisStudent()
        {
            InitializeComponent();
        }

        public void setupfield(int student_id, string name)
        {
            this.student_id = student_id;
            label1.Text = name;
        }

        // setup the labels and button
        public void init()
        {
            try
            {
                bookmanager.ListOfBooksNotYetReturnedByThisStudent(student_id);
            }
            catch
            {
                MessageBox.Show(bookmanager.message);
            }
            dt = bookmanager.getDt();
            font = new Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            bookTitle = new Label[bookmanager.getTotalRowRetrieved()];
            returnButton = new Button[bookmanager.getTotalRowRetrieved()];

            for (int i = 0; i < bookmanager.getTotalRowRetrieved(); i++)
            {
                // Labels
                bookTitle[i] = new Label();
                bookTitle[i].AutoSize = true;
                bookTitle[i].Location = new Point(5, 20 + ((i * 30) + 8));
                bookTitle[i].Text = dt.Rows[i].ItemArray[1].ToString();
                this.Controls.Add(bookTitle[i]);

                // Buttons
                returnButton[i] = new Button();
                returnButton[i].AutoSize = true;
                returnButton[i].Location = new Point(5, 40 + ((i * 30) + 5));
                returnButton[i].Text = "Return";
                returnButton[i].Tag = int.Parse(dt.Rows[i].ItemArray[0].ToString());
                returnButton[i].Click += new EventHandler(StudentsOfThisBook_Click);
                this.Controls.Add(returnButton[i]);
            }

            // for dynamic growth in heigth, formula is form size + (button size * total # of buttons)
            this.Size = new Size(320, 64 + (returnButton.Length * 40));
        }

        void StudentsOfThisBook_Click(object sender, EventArgs e)
        {
            // convert the sender object into button, without converting into button you can only get the text representaion of that button being fired
            Button btn = (Button)sender;
            borrowedmanager.Update(null, new object[]{btn.Tag,
                                                      student_id
                                                     });
            // pop up message if there is a fine
            int fine = borrowedmanager.FineOfThisStudent(int.Parse(btn.Tag.ToString()), student_id);
            if (fine > 0)
            {
                MessageBox.Show("Please Pay " + fine, "Warning! Book Borrowed Exceeded");
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BooksOfThisStudent_Load(object sender, EventArgs e)
        {
            bookmanager = new BookManager();
            borrowedmanager = new BorrowedManager();
            init();
        }
    }
}
