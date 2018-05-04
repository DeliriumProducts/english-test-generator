// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Test.cs" company="Delirium Products">
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
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GetAPIResponse;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ZXing;
using AForge.Imaging;
using AForge.Math.Geometry;
using AForge;
using AForge.Imaging.Filters;
using ZXing.QrCode;

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
                    splitByNewLine[i] += "*All Types";
                }               
                string[] splitByAsterisk = splitByNewLine[i].Split(new string[] {"*"}, StringSplitOptions.RemoveEmptyEntries);
                switch (splitByAsterisk[1].TrimEnd()) // converts the word type to its full name, which is used for sending requests
                {
                    case "n":
                        splitByAsterisk[1] = "Noun";
                        break;
                    case "adj":
                        splitByAsterisk[1] = "Adjective";
                        break;
                    case "ad":
                        splitByAsterisk[1] = "Adverb";
                        break;
                    case "v":
                        splitByAsterisk[1] = "Verb";
                        break;
                    case "phr":
                        splitByAsterisk[1] = "Idiomatic";
                        break;
                    case "r":
                        splitByAsterisk[1] = "Residual";
                        break;
                    default:
                        break;
                }
                TestGeneratorForm.test_WordsAndTypes.Add(splitByAsterisk[0], splitByAsterisk[1]);
            }
        }
        public static string Generate(string test_Type, int test_ExcerciseAmount, string test_Name, Dictionary<string, string> test_Words, string region)
        {
            List<string> exercises = new List<string>();
            List<string> answers = new List<string>();
            ConcurrentBag<string> bagOfAnswers = new ConcurrentBag<string>();
            ConcurrentBag<string> bagOfExercises = new ConcurrentBag<string>(); // used when using multiple threads
            TestGeneratorForm.fr.progressBar1.Visible = true;
            TestGeneratorForm.fr.progressBar1.Value = 0;
            TestGeneratorForm.fr.progressBar1.Maximum = TestGeneratorForm.fr.richTextBox2.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length;
            Dictionary<string, string> buffer = new Dictionary<string, string>();
            Random rndm = new Random();
            string finishedTest = "";
            string suggestedAnswerKey = "";
            int n = 1;
            while(n<=test_ExcerciseAmount && test_Words.Count>0) 
            {
                int randomExercise = rndm.Next(0, test_Words.Count-1);
                string key = test_Words.ElementAt(randomExercise).Key;
                string value = test_Words[key];
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>(key, value);
                if (!Utility.IsValidEntry(pair, test_Type))
                {
                    test_Words.Remove(key);
                    continue;
                }
                buffer.Add(key,test_Words[key]);
                test_Words.Remove(key);
                n++;
            }
            test_Words = buffer;
            switch (TestGeneratorForm.generatingSpeed)
            {
                case "Normal":
                    foreach (KeyValuePair<string, string> entry in test_Words)
                    {
                        TestGeneratorForm.fr.progressBar1.Value++;
                        switch (test_Type)
                        {
                           case "Definitions":
                                exercises.Add(Read(Definitions.get(entry.Value, entry.Key)));
                                answers.Add(entry.Key);
                               break;
                           case "Examples":
                                exercises.Add(Regex.Replace(Read(Examples.get(entry.Value, entry.Key)), entry.Key, new string('_', entry.Key.Length), RegexOptions.IgnoreCase));
                                answers.Add(entry.Key);
                                break;
                            case "Words":
                                exercises.Add(entry.Key + new string('.', 50) + " (" + entry.Value.TrimEnd() + ")");
                                break;
                            case "Multi-Choices":
                                char answer = 'A'; // stores the correct answer for each exercise
                                exercises.Add(Regex.Replace(Read(Examples.get(entry.Value, entry.Key)), entry.Key, new string('_', entry.Key.Length), RegexOptions.IgnoreCase) + "\n" + Synonyms.Request(entry.Key, out answer));
                                answers.Add(answer.ToString());
                                break;
                        }
                    }
                    break;
                case "Fast":
                    System.Threading.Tasks.Parallel.ForEach(test_Words, entry =>
                    {
                        switch (test_Type)
                        {
                            case "Definitions":
                                bagOfExercises.Add(Read(Definitions.get(entry.Value, entry.Key)));
                                bagOfAnswers.Add(entry.Key);
                                break;
                            case "Examples":
                                bagOfExercises.Add(Regex.Replace(Read(Examples.get(entry.Value, entry.Key)), entry.Key, new string('_', entry.Key.Length), RegexOptions.IgnoreCase));
                                bagOfAnswers.Add(entry.Key);
                                break;
                            case "Words":
                                bagOfExercises.Add(entry.Key + new string('.', 50) + " (" + entry.Value.TrimEnd() + ")");
                                break;
                            case "Multi-Choices":
                                char answer = 'A'; // stores the correct answer for each exercise
                                bagOfExercises.Add(Regex.Replace(Read(Examples.get(entry.Value, entry.Key)), entry.Key, new string('_', entry.Key.Length), RegexOptions.IgnoreCase) + "\n" + Synonyms.Request(entry.Key, out answer));
                                bagOfAnswers.Add(answer.ToString());
                                break;
                        }
                    });
                    exercises = bagOfExercises.ToList(); // Converts the bagOfExercises variable to List and sets it to exercises variable
                    answers = bagOfAnswers.ToList();
                    break;
            }
            foreach (var exercise in exercises.ToList()) 
            {
                if ((exercise.Contains("Couldn't find ") && (exercise.Contains("NotFound")|| exercise.Contains("ERROR")) && answers.Any()) ||
                    (!(exercise.Contains("_")) && answers.Any() && (test_Type == "Examples" ||
                    test_Type == "Multi-Choices"))) // remove all of the words that were not found in the Oxford Dictionary
                {
                    answers.RemoveAt(exercises.IndexOf(exercise));
                    exercises.Remove(exercise);
                }
            }    
            if (exercises.Count - test_ExcerciseAmount < 0) // if there are not enough words to make a test, lower the exercise amount
            {
                test_ExcerciseAmount -= (test_ExcerciseAmount - exercises.Count);
            }
            finishedTest += "~~~~~" + test_Name + "~~~~~\n";
            suggestedAnswerKey += (answers.Any()) ? "~~~~~ Suggested Answer Key ~~~~~\n" : string.Empty;
            string answerKey = "";
            n = 1;
            while (n <= test_ExcerciseAmount)
            {
                int randomExercise = rndm.Next(0, exercises.Count);
                finishedTest += "------------------[Ex. "+ n +"]------------------\n" + exercises[randomExercise] + "\n";
                suggestedAnswerKey += (answers.Any()) ? "[Ex. " + n + "] - " + answers[randomExercise] + "\n" : string.Empty;
                exercises.RemoveAt(randomExercise);
                if (answers.Any())
                {
                    if (test_Type == "Multi-Choices")
                    {
                        answerKey += n.ToString() + "-" + answers[randomExercise] + "\n";
                        AnswerSheet.Generate(test_Name, test_ExcerciseAmount, 1, 4, answerKey);
                    }
                    answers.RemoveAt(randomExercise);
                }
                n++;
            }           
            finishedTest += "\n" + suggestedAnswerKey;
            TestGeneratorForm.fr.progressBar1.Visible = false;
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
                    filteredSource += (lines[i - 1].Contains("EXAMPLES")) ? lines[i] + " \n" : lines[i] + " - \n";
                }
            }
            return filteredSource;
        }
    }
}
