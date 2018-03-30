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
            Bitmap bmp = new Bitmap(720,1280);
            Rectangle innerBorder = new Rectangle(70, 70, 600, 1160);
            Rectangle studentData = new Rectangle(70, 0, 600, 70);
            Graphics g = Graphics.FromImage(bmp);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawRectangle(Pens.Black, innerBorder);
            g.DrawRectangle(Pens.Black, studentData);
            g.DrawString($"UNIT 4 MODULE 2 ; Test Group:", new Font("Calibri", 20), Brushes.Black, studentData, sf);
            sf.Alignment = StringAlignment.Near;
            g.DrawString("\nName and Class Num: ", new Font("Calibri", 20), Brushes.Black, studentData,sf );
            g.Flush();
            bmp.Save("hui.bmp");
        }
    }
}
