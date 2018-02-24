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
        public static string Get(string url) // downloads a certain pastebin, checks if it exists locally, if not - it downloads the latest one
        {
            string pastebin = "";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/pastebin.etg");
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
            LoadUnits(pastebin);
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
                        Form1.fr.comboBox2.Items.Add(currentLine.Substring(3)); // remove the "%%%" and add what's left of the string as a comboBox2 item
                    }
                }
            }
        }
    }
}
