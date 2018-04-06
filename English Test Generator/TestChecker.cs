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
            Bitmap bmp = new Bitmap(720,1280);
            Rectangle innerBorder = new Rectangle(70, 70, 580, 1140);
            Rectangle studentData = new Rectangle(70, 0, 580, 70);
            Graphics g = Graphics.FromImage(bmp);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawRectangle(Pens.Black, innerBorder);
            g.DrawRectangle(Pens.Black, studentData);
            g.DrawString($"{testName}; Test Group:", new Font("Calibri", 20), Brushes.Black, studentData, sf);
            sf.Alignment = StringAlignment.Near;
            g.DrawString("\nName and Class Number: ", new Font("Calibri", 20), Brushes.Black, studentData, sf);
            // DRAWING ANSWER SHEET (TEMP CODE LOCATION)
            g.DrawString(possibleAnswers(possibleAnswersAmount), new Font("Calibri", 20), Brushes.Black, 115, 75);
            int offsetY = 0, offsetRecX = 0, offsetRecY = 0, baseX = 75, baseRecX = 110; ; // offsetY - the offset for drawing the current Exercise number, offsetRecX/Y - the offset for drawing the rectangles
            for (int i = 1; i <= exerciseAmount; i++)
            {
                if (i>44)
                {
                    if(i==45) g.DrawString(possibleAnswers(possibleAnswersAmount), new Font("Calibri", 20), Brushes.Black, 395, 75);

                    baseX = 355;
                    baseRecX = 390;
                }
                g.DrawString(i.ToString(), new Font("Calibri", 20), Brushes.Black, baseX, 100+offsetY);
                for (int j = 0; j < possibleAnswersAmount; j++)
                {
                    g.DrawRectangle(Pens.Black, baseRecX + offsetRecX, 105+offsetRecY, 30, 20);
                    offsetRecX += 45;
                }
                offsetRecX = 0;
                offsetY = (i == 44) ? 0 : offsetY + 25;
                offsetRecY = (i == 44) ? 0 : offsetRecY + 25;
            }
            // END DRAWING ANSWER SHEET
            g.Flush();
            bmp.Save("hui.bmp");
        }

        private void TestChecker_Load(object sender, EventArgs e)
        {
            Launcher.l.Hide();
        }

        string possibleAnswers(int num)
        {
            switch (num)
            {
                case 2:
                    return "A     B";
                case 3:
                    return "A     B     C";
                case 4:
                    return "A     B     C     D";
                case 5:
                    return "A     B     C     D     E";
                default:
                    return "A     B     C     D";
            }
        }
        bool hasSpaceAvailable(int x, int y)
        {
            return (x > 575 || y > 1205) ? false : true;
        }
    }
}
