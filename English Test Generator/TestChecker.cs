using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

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
        public static string filePath;
        public static Bitmap bmp;
        public static OpenFileDialog newDialog;

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
            button1.BackColor = Color.FromArgb(255, 217, 66, 53);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            panel1.BringToFront();
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
            richTextBox1.Text.Trim();
            Dictionary<int, char> answerKey = new Dictionary<int, char>();
            string[] lines = richTextBox1.Text.Split(
     new[] { "\r\n", "\r", "\n" },
     StringSplitOptions.None);
            char value;
            for (int i = 1; i <= lines.Length; i++)
            {
                string[] keyPair = lines[i-1].Trim().Split(new[] { "-" }, StringSplitOptions.None);
                value = keyPair[1][0];
                answerKey.Add(i, value);
            }
            bmp = new Bitmap(newDialog.FileName);
            BarcodeReader barcodeReader = new BarcodeReader();
            barcodeReader.Options.TryHarder = true;
            var barcodeBitmap = (Bitmap)bmp;
            Result barcodeResult = barcodeReader.Decode(barcodeBitmap);
            float Ax, Ay, Bx, By;
            Ax = barcodeResult.ResultPoints[1].X-23;
            Ay = barcodeResult.ResultPoints[1].Y-23;
            Bx = bmp.Width / 2;
            By = bmp.Height / 2;
            Graphics g = Graphics.FromImage(bmp);

            float lineLength = (float)Math.Sqrt(Math.Pow(Ay, 2) + Math.Pow(Ax, 2));
            g.DrawLine(Pens.Pink, new PointF(bmp.Width/2,bmp.Height/2), new PointF(Ax,Ay));

            MessageBox.Show(lineLength.ToString());
            bmp.Save("BeforeRotation.bmp");
            /*
             * 
             */
            bmp = Utility.RotateBMP(bmp, Ax, Ay, Bx, By);
            testID = barcodeResult?.Text;
            // bmp - stores the loaded image which will be used later on to recognize human marks
            MessageBox.Show(Test.Check(bmp, testID, answerKey).ToString()+"/"+answerKey.Count+" points");
        }
    }
}
