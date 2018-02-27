// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Test.cs" company="Company">
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GetAPIResponse;

namespace English_Test_Generator
{
    class Test
    {
        public static void FillDictionary(string test_Words)
        {
            string[] splitByNewLine = test_Words.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splitByNewLine.Length; i++)
            {
                if (!splitByNewLine[i].Contains("*")) // for all types
                {
                    splitByNewLine[i] += "*a";
                }               
                string[] splitByAsterisk = splitByNewLine[i].Split(new string[] {"*"}, StringSplitOptions.RemoveEmptyEntries);
                switch (splitByAsterisk[1]) // converts the word type to its full name, which is used for sending requests
                {
                    case "n":
                        splitByAsterisk[1] = "noun";
                        break;
                    case "adj":
                        splitByAsterisk[1] = "adjective";
                        break;
                    case "ad":
                        splitByAsterisk[1] = "adverb";
                        break;
                    case "v":
                        splitByAsterisk[1] = "verb";
                        break;
                    case "phr":
                        splitByAsterisk[1] = "idiomatic";
                        break;
                    default:
                        break;
                }
                Form1.test_WordsAndTypes.Add(splitByAsterisk[0], splitByAsterisk[1]);
            }
        }
        public static string Generate(string test_Type, int test_ExcerciseAmount, string test_Name, Dictionary<string, string> test_Words, string region)
        {
            List<string> exercises = new List<string>();
            string finishedTest = "";
            Form1.fr.progressBar1.Visible = true;
            foreach (KeyValuePair<string, string> entry in test_Words)
            {
                Form1.word_id = entry.Key;
                Form1.word_type = entry.Value;
                switch (test_Type)
                {
                    case "Definitions":
                        exercises.Add(Read(Definitions.get(Form1.word_type, Form1.word_id)));
                        break;
                    case "Examples":
                        exercises.Add(Read(Examples.get(Form1.word_type, Form1.word_id)).Replace(Form1.word_id, new string('_', Form1.word_id.Length)));
                        break;
                }
            } 
            foreach (var exercise in exercises.ToList()) // remove all of the words that were not found in the Oxford Dictionary
            {
                if (exercise.StartsWith("Couldn't find "))
                {
                    exercises.Remove(exercise);
                }
            }    
            if (exercises.Count() - test_ExcerciseAmount < 0) // if there are not enough words to make a test, lower the exercise amount
            {
                test_ExcerciseAmount -= (test_ExcerciseAmount - exercises.Count());
            }
            int n = 1;
            Random rndm = new Random();
            while (n<=test_ExcerciseAmount)
            {
                int randomExercise = rndm.Next(0, exercises.Count()+1);
                finishedTest += "------------------[Ex №"+ n +"]------------------\n" + exercises[randomExercise] + "\n\n";
                exercises.RemoveAt(randomExercise);
                n++;
            }
            return finishedTest;
        }
        public static string Read(string source) // algorithm for reading the returned string from GetAPIResponse
        {
            string[] lines = source.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string filteredSource = "";
            using (StringReader sr = new StringReader(source))
            {
                for (int i = 1; i <= lines.Length; i += 2)
                {
                    filteredSource += lines[i] + "\n";
                }
            }
            return filteredSource;
        }
    }
}
