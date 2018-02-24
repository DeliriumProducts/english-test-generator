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
        public static string GetUnits()
        {
            string units = "";
            string currentLine = "";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/pastebinUnits.etg"); // sets the path to the word
            if (!File.Exists(path)) // checks wheter the file exists or not (has been downloaded or not)
            {
                try // try to download the string and if there isn't an internet connection, warn the user
                {
                    using (WebClient wc = new WebClient())
                    {
                        units = wc.DownloadString(@"https://pastebin.com/raw/szdPcs2Q"); // downloads the latest pastebin                        
                    }
                    using (StreamWriter sw = new StreamWriter(path, false))
                    {
                        sw.WriteLine(units); // save it to disk
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
                        units = wc.DownloadString(@"https://pastebin.com/raw/szdPcs2Q"); // download the latest pastebin
                    }
                        if (units != File.ReadAllText(path)) // read the old file and compare it to the downloaded one
                        {
                            using (StreamWriter sw = new StreamWriter(path, false))
                            {
                                sw.WriteLine(units); // if there's a difference, that would mean that the saved one is outdated and must be replaced                            
                            }
                        }                                                           
                }
                catch (Exception)
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        units = sr.ReadToEnd(); // read the old file                      
                    }
                }               
            }
            using (StringReader sr = new StringReader(units))
            {
                while ((currentLine = sr.ReadLine()) != null) // loop to add all of the units to comboBox2
                {
                    if (currentLine.Contains("%%%")) // if the current line contains "%%%"
                    {
                        Form1.fr.comboBox2.Items.Add(currentLine.Substring(3)); // remove the "%%%" and add what's left of the string as a comboBox2 item
                    }
                }
            }
            return units;
        }
    }
}
