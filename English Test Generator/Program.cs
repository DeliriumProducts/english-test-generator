using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace English_Test_Generator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static void Update()
        {
            if (Pastebin.CheckForUpdate())
            {
                if (MessageBox.Show("New version available, would you like to download it?", "Update available!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    using (WebClient wc = new WebClient())
                    {
                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"ETG " + Pastebin.Get("https://pastebin.com/raw/6aupTvgs", "version"));
                        wc.DownloadFile("https://gitlab.com/simo3003/EnglishTestGenerator/repository/master/archive.zip", path + ".zip");
                        System.IO.Compression.ZipFile.ExtractToDirectory(path + ".zip", path);
                        Process.Start(Directory.GetDirectories(path).First() + @"\English Test Generator\bin\Debug\English Test Generator.exe");
                        Application.Exit();
                    }
                }
            }
            else
            {
                MessageBox.Show("You are already running the latest version of ETG!", "No update available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
