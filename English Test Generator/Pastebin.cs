// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pastebin.cs" company="Company">
//
// Copyright (C) 2018 {Company}
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace English_Test_Generator
{
    class Pastebin
    {
        public static string Get(string url, string name) // downloads a certain pastebin, checks if it exists locally, if not - it downloads the latest one
        {
            string pastebin = "";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/Pastebin/" + name + @".etg");
            if (!File.Exists(path)) // checks wheter the file exists or not (has been downloaded or not)
            {
                try // try to download the string and if there isn't an internet connection, warn the user
                {
                    using (WebClient wc = new WebClient())
                    {
                        pastebin = wc.DownloadString(url); // downloads the latest pastebin                        
                    }
                    using (StreamWriter sw = new StreamWriter(path, false))
                    {
                        sw.WriteLine(pastebin); // save it to disk
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to connect to the internet. Restart the program with internet connectivity at least once!","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);  
                }  
            }
            else // in the event that it already exists / has been downloaded
            {
                try // try to download the string and if there isn't an internet connection, use the old one
                {
                    using (WebClient wc = new WebClient())
                    {
                        pastebin = wc.DownloadString(url); // download the latest pastebin
                    }
                        if (pastebin != File.ReadAllText(path)) // read the old file and compare it to the downloaded one
                        {
                            using (StreamWriter sw = new StreamWriter(path, false))
                            {
                                sw.WriteLine(pastebin); // if there's a difference, that would mean that the saved one is outdated and must be replaced                            
                            }
                        }                                                           
                }
                catch (Exception)
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        pastebin = sr.ReadToEnd(); // read the old file                      
                    }
                }               
            }
            return pastebin;
        }
        public static void LoadUnits(string units)
        {
            string currentLine = "";
            using (StringReader sr = new StringReader(units))
            {
                while ((currentLine = sr.ReadLine()) != null) // loop to add all of the units to comboBox2
                {
                    if (currentLine.Contains("%%%")) // if the current line contains "%%%"
                    {
                        TestGeneratorForm.fr.comboBox2.Items.Add(currentLine.Substring(3)); // remove the "%%%" and add what's left of the string as a comboBox2 item
                    }
                }
            }
        }
        public static bool CheckForUpdate()
        {
            return Get("https://pastebin.com/raw/6aupTvgs", "version") != Properties.Settings.Default.userVersion;
        }
    }
}
