﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" company="Delirium Products">
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GetAPIResponse;
using JSON_lib;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace English_Test_Generator
{
    public partial class TestGeneratorForm : Form
    {
        //-----GLOBAL VARIABLES-----
        public static string appId = Properties.Settings.Default.app_Id; // API ID 
        public static string appKey = Properties.Settings.Default.app_Key; // API Key 
        public static string wordId = ""; // used for storing the current word in the dictionary
        public static string wordType = ""; // used for the current lexical category of a certain word
        public static string wordPrev = ""; // used to store the previous (user's "original") word in the dictionary panel
        public static string dictionary_Result = ""; // result from the dictionary 
        public static TestGeneratorForm fr = new TestGeneratorForm(); // used to change controls from different classes  
        //-----USER PREFERENCES-----
        public static string region = Properties.Settings.Default.userRegion; // user defined region (American / British English) 
        public static string transToLanguage = Properties.Settings.Default.transToLanguage;
        public static string generatingSpeed = Properties.Settings.Default.generatingSpeed;
        public static int rate = Properties.Settings.Default.userRate + 10; // user defined speed of tts
        public static int volume = Properties.Settings.Default.userVolume; // user defined volume of tts
        public static string userTheme = Properties.Settings.Default.userTheme;
        public static string userEditor = Properties.Settings.Default.userEditor;
        //-----TEST VARIABLES-----
        public static Test test = new Test();
        public static string units = "";
        //-----FORM CONSTRUCTOR-----
        public TestGeneratorForm()
        {
            InitializeComponent();
            fr = this; // makes a reference to this form so it can be accessed from different classes
        }
        //-----FORM LOAD SETTINGS-----
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.BringToFront(); // brings the dictionary panel to the front
            button1.BackColor = Color.FromArgb(255, 217, 66, 53); // changes color of the button
            button2.BackColor = Color.FromArgb(255, 20, 20, 20); // changes color of the button
            button3.BackColor = Color.FromArgb(255, 20, 20, 20); // changes color of the button
            button4.BackColor = Color.FromArgb(255, 20, 20, 20); // changes color of the button
            textBox1.AutoSize = false; // disable autosize because windows forms are bad
            textBox1.Height = comboBox1.Height; // set the height so that it matches with the combobox            
            ActiveControl = textBox1; // focus on textBox1
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center; // centers the text on richTextBox1
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center; // centers the text on richTextBox3
            comboBox1.SelectedIndex = 0; // select first value
            checkBox1.Checked = Properties.Settings.Default.autoUpdate; // changes checkBox1 value to match user preference
            checkBox2.Checked = Properties.Settings.Default.autoSearch; // changes checkBox2 value to match user preference
            monoFlat_TrackBar1.Value = volume; // changes trackbar1 value to match user preference
            monoFlat_TrackBar2.Value = rate; // changes trackbar2 value to match user preference
            label16.Text = "Volume: " + monoFlat_TrackBar1.Value; // changes label16 value to match user preference
            label17.Text = "Speed: " + monoFlat_TrackBar2.Value; // changes label17 value to match user preference
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/Examples")); // creates directory in MyDocuments for cache
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/Definitions")); // creates directory in MyDocuments for cache
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/Pastebin")); // creates directory in MyDocuments for cache
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/Synonyms")); // creates directory in MyDocuments for cache
            units = Pastebin.Get("https://pastebin.com/raw/szdPcs2Q", "pastebinWordsAndUnits");
            comboBox2 = Utility.LoadUnits(units, comboBox2);
			if (!Utility.HasRequestsLeft(appId, appKey)) Utility.GetNewCredentials();
            if (Properties.Settings.Default.autoUpdate)
            {                
                new Thread(() => { Program.Update(); }).Start();
            }
            if (!IsApplicationInstalled.Check("Notepad++")) // check if notepad++ is installed and if it's not - removes radiobutton6
            {              
                radioButton6.Visible = false;
                radioButton8.Location = new Point(17, 96);
            }            
            switch (region) // change borders to user preference
            {
                case "gb":
                    button9.FlatAppearance.BorderSize = 1; 
                    button10.FlatAppearance.BorderSize = 0;
                    break;
                case "us":
                    button9.FlatAppearance.BorderSize = 0; 
                    button10.FlatAppearance.BorderSize = 1;
                    break;
            }
            switch (userEditor) // changes radiobuttons in settings to user preference
            {
                case "notepad.exe":
                    radioButton5.Checked = true;
                    break;
                case "notepad++.exe":
                    radioButton6.Checked = true;
                    break;
                case "MS Word":
                    radioButton7.Checked = true;
                    break;
                case "wordpad":
                    radioButton8.Checked = true;
                    break;
            }
            switch (transToLanguage) // changes the label to match the current transToLanguage
            {
                case "en":
                    label8.Text = "Bulgarian to English";
                    webBrowser1.Navigate("https://translate.google.com/#bg/en/");
                    break;
                case "bg":
                    label8.Text = "English to Bulgarian";
                    webBrowser1.Navigate("https://translate.google.com/#en/bg/");
                    break;
            }
            switch (userTheme)
            {
                case "Dark":
                    foreach (Control panel in Controls) // check each panel or groupbox...
                    {
                        if (panel is Panel)
                        {
                            foreach (Control c in panel.Controls) 
                            {
                                if (c is TextBox || c is RichTextBox || c is ComboBox || c is NumericUpDown) // ... and for each textBox, richTextBox, comboBox and numericUpDown in them
                                {
                                    c.ForeColor = Color.FromArgb(255, 235, 235, 235); // change the color
                                    c.BackColor = Color.FromArgb(255, 29, 29, 29);
                                }
                                if (c is GroupBox)
                                {
                                    foreach (Control cInGroupBox in c.Controls)
                                    {
                                        if (cInGroupBox is TextBox || cInGroupBox is RichTextBox || cInGroupBox is ComboBox || cInGroupBox is NumericUpDown) // ... and for each textBox, richTextBox, comboBox and numericUpDown in them
                                        {
                                            cInGroupBox.ForeColor = Color.FromArgb(255, 235, 235, 235); // change the color
                                            cInGroupBox.BackColor = Color.FromArgb(255, 29, 29, 29);
                                        }
                                    }                                   
                                }
                            }
                        }                       
                    }
                    radioButton9.Checked = true;
                    break;
                case "Light":
                    foreach (Control panel in Controls) // check each panel or groupbox...
                    {
                        if (panel is Panel)
                        {
                            foreach (Control c in panel.Controls)
                            {
                                if (c is TextBox || c is RichTextBox || c is ComboBox || c is NumericUpDown) // ... and for each textBox, richTextBox, comboBox and numericUpDown in them
                                { 
                                    c.ForeColor = Color.FromName("WindowText"); // change the color
                                    c.BackColor = Color.FromName("Window");
                                }
                                if (c is GroupBox)
                                {
                                    foreach (Control cInGroupBox in c.Controls)
                                    {
                                        if (cInGroupBox is TextBox || cInGroupBox is RichTextBox || cInGroupBox is ComboBox || cInGroupBox is NumericUpDown) // ... and for each textBox, richTextBox, comboBox and numericUpDown in them
                                        {
                                            cInGroupBox.ForeColor = Color.FromName("WindowText"); // change the color
                                            cInGroupBox.BackColor = Color.FromName("Window");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    radioButton10.Checked = true;
                    break;
            }
            switch (generatingSpeed)
            {
                case "Fast":
                    radioButton12.Checked = true;
                    break;
            }
            Launcher.l.Hide();
        }
        //-----TABS / PANELS-----
        private void button1_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
            button1.BackColor = Color.FromArgb(255, 217, 66, 53);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button3.BackColor = Color.FromArgb(255, 20, 20, 20);
            button4.BackColor = Color.FromArgb(255, 20, 20, 20);
        }
        private void button2_Click(object sender, EventArgs e)
        {          
            panel2.BringToFront();
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            button2.BackColor = Color.FromArgb(255, 217, 66, 53);
            button3.BackColor = Color.FromArgb(255, 20, 20, 20);
            button4.BackColor = Color.FromArgb(255, 20, 20, 20);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            panel3.BringToFront();
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button3.BackColor = Color.FromArgb(255, 217, 66, 53);
            button4.BackColor = Color.FromArgb(255, 20, 20, 20);
            timer1.Start(); // Timer used for translating
        }
        private void button4_Click(object sender, EventArgs e)
        {
            panel4.BringToFront();
            button1.BackColor = Color.FromArgb(255, 20, 20, 20);
            button2.BackColor = Color.FromArgb(255, 20, 20, 20);
            button3.BackColor = Color.FromArgb(255, 20, 20, 20);
            button4.BackColor = Color.FromArgb(255, 217, 66, 53);
        }
        //-----DICTIONARY-----
        private void button5_Click(object sender, EventArgs e)
        {                
                comboBox3.Visible = false; // hides comboBox3 (used in the case if there is more than one result of a given search
                comboBox3.Items.Clear(); // clears comboBox3's previous items
                wordId = textBox1.Text; // gets the word from textBox1
                wordId = SearchWord.GetCorrectWord(wordId, region); // calls the GetCorretWord method from the class SearchWord to serach the user's word                
                if (wordId != textBox1.Text) // checks if the returned word doesn't match textBox1
                {
                    wordPrev = textBox1.Text;// sets word_prev to match textBox1
                    return; // ends the method
                }
                textBox1.Text = wordId; // sets textBox1 to match word_id                                  
                wordType = comboBox1.Text.ToLower(); // gets the type from comboBox1 and makes it lowercase                
                dictionary_Result = Definitions.Get(wordType, wordId); // calls the get method from the Definitions class to get the definition of the user's word
                richTextBox1.Text = dictionary_Result; // prints out the result in richTextBox1           
        }
        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e) // a combobox will appear if there are more than 1 results for the correct word
        {
            DialogResult dg = MessageBox.Show("Is " + comboBox3.GetItemText(comboBox3.SelectedItem) + " the correct word?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question); 
            if (dg == DialogResult.Yes) { wordId = comboBox3.GetItemText(comboBox3.SelectedItem); textBox1.Text = wordId; comboBox3.Visible = false; }            
        }
        private void richTextBox1_MouseLeave(object sender, EventArgs e) // if the user moves the cursor outside richtTextBox1, it will become smaller
        {
            richTextBox1.Location = new Point(32, 155);
            richTextBox1.Size = new Size(620, 148);          
       }
        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (richTextBox1.Text != string.Empty)
            {
                richTextBox1.Location = new Point(32, 9);
                richTextBox1.Size = new Size(620, 294);
            }
        }
        //-----TEST MAKER-----
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = false;
            richTextBox2.Visible = true;
            richTextBox3.Location = new Point(367, 29);
            richTextBox3.Size = new Size(305, 274);
            label6.Visible = true;
            label7.Location = new Point(362, 3);
            richTextBox2.Clear();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;
            richTextBox2.Text = Utility.GetSpecificUnit(comboBox2.GetItemText(comboBox2.SelectedItem));
            richTextBox2.Visible = false;
            richTextBox3.Location = new Point(228, 29);
            richTextBox3.Size = new Size(444, 274);
            label6.Visible = false;
            label7.Location = new Point(223, 3);
        }
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e) // gets the words only for the specified unit
        {
            richTextBox2.Text = Utility.GetSpecificUnit(comboBox2.GetItemText(comboBox2.SelectedItem));     
        }
        private void button6_Click(object sender, EventArgs e)
        {
            button7.BackgroundImage = Properties.Resources.greyPrinter;
            button7.Enabled = false;
            test.Name = textBox2.Text; // sets the name of the test
            test.Type = comboBox4.Text; // sets the type of the test
            test.ExcerciseAmount = int.Parse(numericUpDown1.Text);// sets the excercise amount for the test
            test.WordsAndTypes = test.FillDictionary(richTextBox2.Text); // makes the words and types into a dictionary
            test.Result = test.Generate(test); // generates the test
            richTextBox3.Text = test.Result;
            button7.BackgroundImage = Properties.Resources.redPrinter;
            button7.Enabled = true;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            button7.BackgroundImage = Properties.Resources.redPrinter;
            button7.Enabled = true;
            if (test.Result != "")
            {                
                string path = test.Name + ".txt";
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(test.Result);
                }
                if (userEditor == "MS Word")
                {
                    MSWord msword = new MSWord();
                    msword.Show();
                }
                else
                {
                    Process.Start(userEditor, path);
                }
            }
        }
        //-----ABOUT-----
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }
        //-----TRANSLATOR SETTINGS-----
        private void button8_Click(object sender, EventArgs e)
        {
            if (transToLanguage == "en") //if current setting is set to "en" (bg -> en), set it to "bg" (en -> bg)
            {
                label8.Text = "English to Bulgarian";
                Properties.Settings.Default.transToLanguage = "bg";
                Properties.Settings.Default.Save(); // saves the user's settings
                webBrowser1.Navigate("https://translate.google.com/#en/bg/"); // Changes the webBrowser's URL based on user's preference
            }
            else 
            {
                label8.Text = "Bulgarian to English";
                Properties.Settings.Default.transToLanguage = "en";
                Properties.Settings.Default.Save(); // saves the user's settings
                webBrowser1.Navigate("https://translate.google.com/#bg/en/"); // Changes the webBrowser's URL based on user's preference
            }
            // The following code swaps the text from the Rich text boxes
            richTextBox4.Text = richTextBox5.Text;
            richTextBox5.Text = "";
            transToLanguage = Properties.Settings.Default.transToLanguage;
        }
        //-----USER LANGUAGE SETTINGS-----
        private void button9_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.userRegion = "gb"; // changes default language to british english (used for API requests)
            region = "gb";
            button9.FlatAppearance.BorderSize = 1; // change borders to user preference
            button10.FlatAppearance.BorderSize = 0;
            Properties.Settings.Default.Save(); // saves the user's settings
        }
        private void button10_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.userRegion = "us"; // changes default language to american english (used for API requests)
            region = "us";
            button9.FlatAppearance.BorderSize = 0; // change borders to user preference
            button10.FlatAppearance.BorderSize = 1;
            Properties.Settings.Default.Save(); // saves the user's settings
        }
        //-----USER THEME SETTINGS-----
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userTheme = "Dark";
            foreach (Control panel in Controls) // check each panel or groupbox...
            {
                if (panel is Panel)
                {
                    foreach (Control c in panel.Controls)
                    {
                        if (c is TextBox || c is RichTextBox || c is ComboBox || c is NumericUpDown) // ... and for each textBox, richTextBox, comboBox and numericUpDown in them
                        {
                            c.ForeColor = Color.FromArgb(255, 235, 235, 235); // change the color
                            c.BackColor = Color.FromArgb(255, 29, 29, 29);
                        }
                        if (c is GroupBox)
                        {
                            foreach (Control cInGroupBox in c.Controls)
                            {
                                if (cInGroupBox is TextBox || cInGroupBox is RichTextBox || cInGroupBox is ComboBox || cInGroupBox is NumericUpDown) // ... and for each textBox, richTextBox, comboBox and numericUpDown in them
                                {
                                    cInGroupBox.ForeColor = Color.FromArgb(255, 235, 235, 235); // change the color
                                    cInGroupBox.BackColor = Color.FromArgb(255, 29, 29, 29);
                                }
                            }
                        }
                    }
                }
            }
            Properties.Settings.Default.Save();
        }
        private void radioButton10_CheckedChanged_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.userTheme = "Light";
            foreach (Control panel in Controls) // check each panel or groupbox...
            {
                if (panel is Panel)
                {
                    foreach (Control c in panel.Controls)
                    {
                        if (c is TextBox || c is RichTextBox || c is ComboBox || c is NumericUpDown) // ... and for each textBox, richTextBox, comboBox and numericUpDown in them
                        {
                            c.ForeColor = Color.FromName("WindowText"); // change the color
                            c.BackColor = Color.FromName("Window");
                        }
                        if (c is GroupBox)
                        {
                            foreach (Control cInGroupBox in c.Controls)
                            {
                                if (cInGroupBox is TextBox || cInGroupBox is RichTextBox || cInGroupBox is ComboBox || cInGroupBox is NumericUpDown) // ... and for each textBox, richTextBox, comboBox and numericUpDown in them
                                {
                                    cInGroupBox.ForeColor = Color.FromName("WindowText"); // change the color
                                    cInGroupBox.BackColor = Color.FromName("Window");
                                }
                            }
                        }
                    }
                }
            }
            Properties.Settings.Default.Save();
        }
        //-----USER AUTO UPDATE SETTINGS-----
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) 
            {              
                Properties.Settings.Default.autoUpdate = true;
                Properties.Settings.Default.Save(); // saves the user's settings
            }
            else 
            {
                Properties.Settings.Default.autoUpdate = false;
                Properties.Settings.Default.Save(); // saves the user's settings
            }            
        }
        //-----USER TEXT EDITOR SETTINGS-----
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userEditor = "notepad.exe";
            Properties.Settings.Default.Save();
            userEditor = Properties.Settings.Default.userEditor;
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userEditor = "notepad++.exe";
            Properties.Settings.Default.Save();
            userEditor = Properties.Settings.Default.userEditor;
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userEditor = "MS Word";
            Properties.Settings.Default.Save();
            userEditor = Properties.Settings.Default.userEditor;
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.userEditor = "wordpad";
            Properties.Settings.Default.Save();
            userEditor = Properties.Settings.Default.userEditor;
        }
        //-----USER TEXT TO SPEECH SETTINGS------
        private void monoFlat_TrackBar2_ValueChanged()
        {
            rate = monoFlat_TrackBar2.Value - 10; 
            label17.Text = "Speed: " + monoFlat_TrackBar2.Value;
            Properties.Settings.Default.userRate = rate;
            Properties.Settings.Default.Save();
        }
        private void monoFlat_TrackBar1_ValueChanged()
        {
            volume = monoFlat_TrackBar1.Value;
            label16.Text = "Volume: " + monoFlat_TrackBar1.Value;
            Properties.Settings.Default.userVolume = volume;
            Properties.Settings.Default.Save();
        }
        //-----USER SPEECH TO TEXT SETTINGS------
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.autoSearch = checkBox2.Checked;
            Properties.Settings.Default.Save();
        } 
        //----SETTINGS PAGES-----
        private void button16_Click(object sender, EventArgs e) // p1 -> p2
        {
            panel4.BringToFront();          
        }
        private void button14_Click(object sender, EventArgs e) // p2 -> p1
        {
            panel5.BringToFront();           
        }
        //-----SPEAK BUTTON-----
        private void button12_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer(); // object for text to speech
            synthesizer.Rate = rate; // sets the tts' speech speed
            synthesizer.Volume = volume; // sets the tts' speech volume            
            synthesizer.Speak(textBox1.Text); // speaks out the user's word            
        }
        //-----LISTEN BUTTON-----
        private void button17_Click(object sender, EventArgs e)
        {
            SpeechRecognition sr = new SpeechRecognition();
            Thread thr = new Thread(new ThreadStart(sr.Recognize));
            thr.Start();
        }
        //-----MISCELLANEOUS-----
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (button3.BackColor != Color.FromArgb(255, 217, 66, 53)) timer1.Stop();
            if (transToLanguage == "en")
            {
                if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                {
                    webBrowser1.Document.GetElementById("source").InnerText = richTextBox4.Text;
                    string result = webBrowser1.Document.GetElementById("gt-res-dir-ctr").InnerText;
                    richTextBox5.Text = result;
                }
            }
            if (transToLanguage == "bg")
            {
                if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                {
                    webBrowser1.Document.GetElementById("source").InnerText = richTextBox4.Text;
                    string result = webBrowser1.Document.GetElementById("gt-res-dir-ctr").InnerText;
                    richTextBox5.Text = result;
                }
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            Program.Update();
        }
        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                button5.PerformClick();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.D1:
                        button1.PerformClick();
                        break;
                    case Keys.D2:
                        button2.PerformClick();
                        break;
                    case Keys.D3:
                        button3.PerformClick();
                        break;
                    case Keys.D4:
                        button4.PerformClick();
                        break;
                }
            }         
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Compared to \"Normal\" Test Generating, this option generates tests much faster, but with the lack of progress indication.\nFast Test Generating is achieved by using more of your Computer Resources.", "Info", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            generatingSpeed = "Normal";
            Properties.Settings.Default.generatingSpeed = generatingSpeed;
            Properties.Settings.Default.Save();
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            generatingSpeed = "Fast";
            Properties.Settings.Default.generatingSpeed = generatingSpeed;
            Properties.Settings.Default.Save();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true ;
            Launcher.l.Show();
            Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
