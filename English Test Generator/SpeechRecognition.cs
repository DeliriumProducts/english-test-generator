// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpeechRecognition.cs" company="Delirium Products">
//
// Copyright (C) 2018 {Delirium Products
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
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace English_Test_Generator
{
    class SpeechRecognition
    {       
        public void SetText(string text) // used when cross-threading to set textBox1's text
        { 
            TestGenerator.fr.Invoke(new Action(() => { TestGenerator.fr.textBox1.Text = text; }));
        }
        private void ClickExecution()
        {            
            TestGenerator.fr.Invoke(new Action(() => { TestGenerator.fr.button5.PerformClick(); }));
        }
        public void Recognize()
        {
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(); // creates a new Recognizer for voice commands
            Grammar dictationGrammar = new DictationGrammar();
            recognizer.LoadGrammar(dictationGrammar); // loads default grammar
            try
            {
                SetText("Listening...");
                recognizer.SetInputToDefaultAudioDevice(); // sets the input device to the user's default input device
                RecognitionResult result = recognizer.Recognize(); // used to capture the result
                SetText(result.Text); // sets the result to the search box(textBox1)
                if (Properties.Settings.Default.autoSearch) ClickExecution(); // checks if the user wants automatic search after the word is captured
                recognizer.UnloadAllGrammars();
            }
            catch (InvalidOperationException exception)
            {
                SetText(String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?\r\n{0} - {1}.", exception.Source, exception.Message)); // throws an error if something goes wrong
            }
        }
    }
}
