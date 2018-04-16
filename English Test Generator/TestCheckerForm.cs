using System;
using System.Collections;
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
            string[] lines = richTextBox1.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            char value;
            for (int i = 1; i <= lines.Length; i++)
            {
                string[] keyPair = lines[i - 1].Trim().Split(new[] { "-" }, StringSplitOptions.None);
                value = keyPair[1][0];
                answerKey += i.ToString() + "-" + value + "\n";
            }
            Test.GenerateAnswerSheet(testName, exerciseAmount, testGroupsAmount, possibleAnswersAmount, answerKey); // make it a non-void method and return and save / open the finished bitmap
            if(MessageBox.Show("Answer sheet with name " + testName + " successfully generated! Would you like to open it?","Answer Sheet", MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Process.Start(Application.StartupPath+$@"\{testName}.bmp");
            }
        }

        private void TestChecker_Load(object sender, EventArgs e)
        {
            Launcher.l.Hide();
            button2.BackColor = Color.FromArgb(255, 217, 66, 53);
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            panel2.BringToFront();
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            newDialog = new OpenFileDialog();
            newDialog.Title = "Open Answer Sheet";
            newDialog.InitialDirectory = Application.ExecutablePath;
            if(newDialog.ShowDialog()==DialogResult.OK)
            {
                textBox5.Text = newDialog.FileName;
                filePath = newDialog.FileName;
            }
        }

        private void monoFlat_Button2_Click(object sender, EventArgs e)
        {
            int timesRotated = 0;
            Dictionary<int, char> answerKey = new Dictionary<int, char>();
            bmp = new Bitmap(newDialog.FileName);
            int Ax, Ay, Bx, By;
            if(!Utility.ReadQRCode(bmp, out Result barcodeResult, timesRotated))
            {
                MessageBox.Show("Please adjust the answer sheet", "QR Code Could not be found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Ax = (int)barcodeResult.ResultPoints[1].X;
            Ay = (int)barcodeResult.ResultPoints[1].Y;
            Bx = (int)barcodeResult.ResultPoints[2].X;
            By = (int)barcodeResult.ResultPoints[2].Y;
            Graphics g = Graphics.FromImage(bmp);
            float k = bmp.Width/720.0f;
            //float k = bmp.Width/720.0f;
            bmp.Save("BeforeRotation.bmp");
            bmp = Utility.RotateBMP(bmp, Ax, Ay, Bx, By);
            testID = Utility.Decrypt(barcodeResult?.Text);
            testID.TrimEnd();
            string[] lines = testID.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i <= lines.Length-1; i++)
            {
                string[] currentLine = lines[i].Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                answerKey.Add(i, currentLine[1][0]);
            }
            MessageBox.Show("Student has scored: " + Test.Check(bmp, testID, answerKey, k).ToString()+"/"+answerKey.Count+" points");
        }
    }
}
