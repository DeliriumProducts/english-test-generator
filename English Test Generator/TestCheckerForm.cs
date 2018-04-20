using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
namespace English_Test_Generator
{
    public partial class TestCheckerForm : Form
    {
        public static TestCheckerForm tc = new TestCheckerForm();
        public static string testName;
        public static int exerciseAmount;
        public static int testGroupsAmount;
        public static int possibleAnswersAmount;
        public static string testID;
        public static string filePath;
        public static Bitmap bmp;
        public static OpenFileDialog newDialog;
        public TestCheckerForm()
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
            richTextBox1.Text.Trim();
            string answerKey = "";
            string[] lines = richTextBox1.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            char value;
            for (int i = 1; i <= lines.Length; i++)
            {
                string[] keyPair = lines[i - 1].Trim().Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                value = keyPair[1][0];
                answerKey += i.ToString() + "-" + value + "\n";
            }
            Test.GenerateAnswerSheet(testName, exerciseAmount, testGroupsAmount, possibleAnswersAmount, answerKey); // make it a non-void method and return and save / open the finished bitmap
            if (MessageBox.Show("Answer sheet with name " + testName + " successfully generated! Would you like to open it?", "Answer Sheet", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Process.Start(Application.StartupPath + $@"\{testName}.bmp");
            }
        }

        private void TestChecker_Load(object sender, EventArgs e)
        {
            Launcher.l.Hide();
            button2.BackColor = Color.FromArgb(255, 217, 66, 53);
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            panel2.BringToFront();
        }
      
        private void monoFlat_Button2_Click(object sender, EventArgs e)
        {
        }
        private void pictureBox2_DragDrop(object sender, DragEventArgs e)
        {
         
        }

        private void pictureBox2_DragEnter(object sender, DragEventArgs e)
        {
      
        }

        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
   string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            ConcurrentBag<string> studentsResults = new ConcurrentBag<string>();
            Parallel.ForEach(filePaths, filePath =>
             {
                 Bitmap answerSheet;
                 if (filePath.EndsWith(".pdf")) 
                     answerSheet = Utility.ConvertPDFToPNG(filePath);
                 Utility.InitializeAnswerSheet(out studentsResults, filePath);
             });
        }

        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
    } 
}
