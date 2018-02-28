using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace English_Test_Generator
{
    public partial class MSWord : Form
    {
        public MSWord()
        {
            InitializeComponent();
        }

        private void MSWord_Load(object sender, EventArgs e)
        {
            radRichTextEditor1.Text = Form1.test_Result;
        }
    }
}
