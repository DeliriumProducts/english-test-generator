using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace English_Test_Generator
{
    public partial class TestChecker : Form
    {
        public static TestChecker tc = new TestChecker();
        public static string testName;
        public static int exerciseAmount;
        public static int testGroupsAmount;
        public static int possibleAnswersAmount;
        public static string testID;
        public TestChecker()
        {
            InitializeComponent();
            tc = this;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            button2.BackColor = Color.FromArgb(255, 217, 66, 53);
            panel2.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button1.BackColor = Color.FromArgb(255, 217, 66, 53);
            panel1.BringToFront();
        }

        private void TestChecker_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Launcher.l.Show();
            Hide();
        }

        private void monoFlat_Button1_Click(object sender, EventArgs e)
        {
            testName = textBox1.Text;
            exerciseAmount = Convert.ToInt32(numericUpDown1.Value);
            testGroupsAmount = Convert.ToInt32(numericUpDown2.Value);
            possibleAnswersAmount = Convert.ToInt32(numericUpDown3.Value);
            Test.GenerateAnswerSheet(testName, exerciseAmount, testGroupsAmount, possibleAnswersAmount); // make it a non-void method and return and save / open the finished bitmap
        }

        private void TestChecker_Load(object sender, EventArgs e)
        {
            Launcher.l.Hide();
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp;
            string filePath;
            OpenFileDialog newDialog = new OpenFileDialog();
            newDialog.Title = "Open Answer Sheet";
            newDialog.InitialDirectory = @"C:\Users\Любо Любчев\Desktop\EnglishTestGenerator\EnglishTestGenerator\English Test Generator\bin\Debug";
            if(newDialog.ShowDialog()==DialogResult.OK)
            {
                textBox5.Text = newDialog.FileName;
                filePath = newDialog.FileName;
            }
            // TEMP CODE LOCATION
            bmp = new Bitmap(newDialog.FileName); // bmp - stores the loaded image which will be used later on to recognize human marks
            Bitmap box = new Bitmap(29, 19);
            Graphics g = Graphics.FromImage(box);
            box = bmp.Clone(new Rectangle(110,105,29,19),box.PixelFormat);
            box.Save("shittingAround.bmp");
            g.Flush();
        }

        private void monoFlat_Button2_Click(object sender, EventArgs e)
        {
            exerciseAmount = Convert.ToInt32(textBox4.Text);
            possibleAnswersAmount = Convert.ToInt32(textBox3.Text);
            testGroupsAmount = Convert.ToInt32(textBox2.Text);
            testID = $"{exerciseAmount}/{possibleAnswersAmount}/{testGroupsAmount}";
           // TO DO: Test.Check(testID)
        }
    }
}
