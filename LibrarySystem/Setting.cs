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
    public partial class Setting : Form
    {
        private SettingManager sm;

        public Setting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sm.updateSetting(textBox1.Text, textBox2.Text, int.Parse(textBox3.Text), int.Parse(textBox4.Text), int.Parse(textBox5.Text));
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            sm = new SettingManager();
            textBox1.Text = sm.username();
            textBox2.Text = sm.password();
            textBox3.Text = sm.fine().ToString();
            textBox4.Text = sm.daysallowed().ToString();
            textBox5.Text = sm.maxbookallowed().ToString();
        }
    }
}
