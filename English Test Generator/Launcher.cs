using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace English_Test_Generator
{
    public partial class Launcher : Form
    {
        public static Launcher l = new Launcher();
        public Launcher()
        {
            InitializeComponent();
            l = this;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://deliriumproducts.tk");
        }

        private void Launcher_Load(object sender, EventArgs e)
        {

        }

        private void monoFlat_Button1_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();

        }
    }
}
