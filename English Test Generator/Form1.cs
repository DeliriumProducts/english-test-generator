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

        //-----GLOBAL VARIABLES-----
        public static string app_Id = "f930a9d7"; // API ID duuh..
        public static string app_Key = "ec116568011054d2efef549e5625959d"; // API Key duuh..
        bool isENGtoBG = true; // Google Translator's default setting
        public static string word_id = "";
        public static string language = Properties.Settings.Default.userLanguage;
        public static string word_type = "";
        public Form1()
        {
            InitializeComponent();
           
        }
        //-----FORM LOAD SETTINGS-----
        private void Form1_Load(object sender, EventArgs e)
        {          
            panel1.BringToFront();
            button1.BackColor = Color.FromArgb(255, 217, 66, 53);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button3.BackColor = Color.FromArgb(255, 20, 20, 20);
            button4.BackColor = Color.FromArgb(255, 20, 20, 20);
            textBox1.AutoSize = false; // disable autosize because windows forms are bad
            textBox1.Height = comboBox1.Height; // set the height so that it matches with the combobox            
            ActiveControl = textBox1; // focus on textBox1
            comboBox1.SelectedIndex = 0; // select first value
            checkBox1.Checked = Properties.Settings.Default.autoUpdate; // changes checkBox1 value to match user preference
            if (!IsApplicationInstalled.Check("notepad++"))
                // check if notepad++ is installed
            {              
                radioButton6.Visible = false;
                radioButton8.Location = new Point(17, 96);
            }            
            switch (Properties.Settings.Default.userLanguage) // change borders to user preference
            {
                case "en":
                    button9.FlatAppearance.BorderSize = 1; 
                    button10.FlatAppearance.BorderSize = 0;
                    break;
                case "us":
                    button9.FlatAppearance.BorderSize = 0; 
                    button10.FlatAppearance.BorderSize = 1;
                    break;
            }
            switch (Properties.Settings.Default.userEditor) // changes radiobuttons in settings to user preference
            {
                case "notepad.exe":
                    radioButton5.Checked = true;
                    break;
                case "notepad++.exe":
                    radioButton6.Checked = true;
                    break;
                case "MS Word":
                    radioButton7.Checked = true;
                    break;
                case "wordpad.exe":
                    radioButton8.Checked = true;
                    break;
            }
        }
        //-----TAB / PANEL SETTINGS-----
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
        //-----TEST MAKER SETTINGS-----
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
        //-----ABOUT-----
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("2ma murzeli");
        }
        //-----TRANSLATOR SETTINGS-----
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
        //-----USER LANGUAGE SETTINGS-----
        private void button9_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.userLanguage = "en"; // changes default language to british english (used for API requests)
            button9.FlatAppearance.BorderSize = 1; // change borders to user preference
            button10.FlatAppearance.BorderSize = 0;
            Properties.Settings.Default.Save();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.userLanguage = "us"; // changes default language to american english (used for API requests)
            button9.FlatAppearance.BorderSize = 0; // change borders to user preference
            button10.FlatAppearance.BorderSize = 1;
            Properties.Settings.Default.Save();
        }
        //-----USER AUTO UPDATE SETTINGS-----
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {              
                Properties.Settings.Default.autoUpdate = true;
                Properties.Settings.Default.Save();
            }
            else 
            {
                Properties.Settings.Default.autoUpdate = false;
                Properties.Settings.Default.Save();
            }
            
        }
        //-----USER TEXT EDITOR SETTINGS-----
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userEditor = "notepad.exe";
            Properties.Settings.Default.Save();
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userEditor = "notepad++.exe";
            Properties.Settings.Default.Save();
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userEditor = "MS Word";
            Properties.Settings.Default.Save();
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userEditor = "wordpad.exe";
            Properties.Settings.Default.Save();
        }
        //-----MISCELLANEOUS-----
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {
            word_id = textBox1.Text;
            word_id = SearchWord.GetCorrectWord(word_id);
            textBox1.Text = word_id;
           
            
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Is " + comboBox3.GetItemText(comboBox3.SelectedItem) + " the correct word?", "Confirmation", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dg == DialogResult.Yes) { word_id = comboBox3.GetItemText(comboBox3.SelectedItem); textBox1.Text = word_id; comboBox3.Visible = false;}                
        }
    }
}
