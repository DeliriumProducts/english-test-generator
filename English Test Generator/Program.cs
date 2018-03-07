// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Delirium Products">
//
// Copyright (C) 2018 Delirium Products
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// This program is used to generate english tests for students / teachers
// 
// Email: simo3003@me.com / lyubo_2317@abv.bg
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"ETG " + Pastebin.Get("https://pastebin.com/raw/6aupTvgs", "version") + ".exe");
                        wc.DownloadFile(wc.DownloadString("https://pastebin.com/raw/ZUS8BfyC"), path);
                        Process.Start(path);
                        Application.Exit();
                    }
                }
            }
            else
            {
                MessageBox.Show("You are running the latest version of ETG!", "No update available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
