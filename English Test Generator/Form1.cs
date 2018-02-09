using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GetAPIResponse;
using JSON_lib;
namespace English_Test_Generator
{
    public partial class Form1 : Form
    {
        string app_Id = "f930a9d7"; // API ID duuh..
        string app_Key = "ec116568011054d2efef549e5625959d"; // API Key duuh..
        bool isENGtoBG = true; // Google Translator's default setting
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {                     
            panel1.BringToFront();
            button1.BackColor = Color.FromArgb(255, 217, 66, 53);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button3.BackColor = Color.FromArgb(255, 20, 20, 20);
            button4.BackColor = Color.FromArgb(255, 20, 20, 20);
            textBox1.AutoSize = false; // disable autosize because windows forms suck
            textBox1.Height = comboBox1.Height; // set the height so that it matches with the combobox            
            ActiveControl = textBox1; // focus on textBox1
            comboBox1.SelectedIndex = 0; // select first value

        }
        private void button1_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
            button1.BackColor = Color.FromArgb(255, 217, 66, 53);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button3.BackColor = Color.FromArgb(255, 20, 20, 20);
            button4.BackColor = Color.FromArgb(255, 20, 20, 20);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            button2.BackColor = Color.FromArgb(255, 217, 66, 53);
            button3.BackColor = Color.FromArgb(255, 20, 20, 20);
            button4.BackColor = Color.FromArgb(255, 20, 20, 20);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            panel3.BringToFront();
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button3.BackColor = Color.FromArgb(255, 217, 66, 53);
            button4.BackColor = Color.FromArgb(255, 20, 20, 20);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            panel4.BringToFront();
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button3.BackColor = Color.FromArgb(255, 20, 20, 20);
            button4.BackColor = Color.FromArgb(255, 217, 66, 53);
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = false;
            richTextBox2.Visible = true;
            richTextBox3.Location = new Point(367, 29);
            richTextBox3.Size = new Size(305, 274);
            label6.Visible = true;
            label7.Location = new Point(362, 3);
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;
            richTextBox2.Visible = false;
            richTextBox3.Location = new Point(228, 29);
            richTextBox3.Size = new Size(444, 274);
            label6.Visible = false;
            label7.Location = new Point(223, 3);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            isENGtoBG = !isENGtoBG;
            if (isENGtoBG)
            {
                label8.Text = "English to Bulgarian";
            }
            else
            {
                label8.Text = "Bulgarian to English";
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            button7.BackgroundImage = Properties.Resources.redPrinter;
            button7.Enabled = true;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            button7.BackgroundImage = Properties.Resources.greyPrinter;
            button7.Enabled = false;
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }     
        private void pictureBox1_Click(object sender, EventArgs e) // About
        {
            MessageBox.Show("2ma murzeli");
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
