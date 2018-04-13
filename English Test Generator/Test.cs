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
                Form1.test_WordsAndTypes.Add(splitByAsterisk[0], splitByAsterisk[1]);
            }
        }
        public static string Generate(string test_Type, int test_ExcerciseAmount, string test_Name, Dictionary<string, string> test_Words, string region)
        {
            List<string> exercises = new List<string>();
            List<string> answers = new List<string>();
            ConcurrentBag<string> bagOfAnswers = new ConcurrentBag<string>();
            ConcurrentBag<string> bagOfExercises = new ConcurrentBag<string>(); // used when using multiple threads
            Form1.fr.progressBar1.Visible = true;
            Form1.fr.progressBar1.Value = 0;
            Form1.fr.progressBar1.Maximum = Form1.fr.richTextBox2.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length;
            string finishedTest = "";
            string suggestedAnswerKey = "";
            switch (Form1.generatingSpeed)
            {
                case "Normal":
                    foreach (KeyValuePair<string, string> entry in test_Words)
                    {
                        Form1.fr.progressBar1.Value++;
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
                        }
                    }
                    break;
                case "Fast":
                    Parallel.ForEach(test_Words, entry =>
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
                        }
                    });
                    exercises = bagOfExercises.ToList(); // Converts the bagOfExercises variable to List and sets it to exercises variable
                    answers = bagOfAnswers.ToList();
                    break;
            }                  
            foreach (var exercise in exercises.ToList()) // remove all of the words that were not found in the Oxford Dictionary
            {
                if (exercise.StartsWith("Couldn't find ") && answers.Any())
                {
                    answers.RemoveAt(exercises.IndexOf(exercise));
                    exercises.Remove(exercise);
                }
            }    
            if (exercises.Count - test_ExcerciseAmount < 0) // if there are not enough words to make a test, lower the exercise amount
            {
                test_ExcerciseAmount -= (test_ExcerciseAmount - exercises.Count);
            }
            int n = 1;
            Random rndm = new Random();
            finishedTest += "~~~~~" + test_Name + "~~~~~\n";
            suggestedAnswerKey += (answers.Any()) ? "~~~~~ Suggested Answer Key ~~~~~\n" : string.Empty;
            while (n <= test_ExcerciseAmount)
            {
                int randomExercise = rndm.Next(0, exercises.Count);
                finishedTest += "------------------[Ex. "+ n +"]------------------\n" + exercises[randomExercise] + "\n";
                suggestedAnswerKey += (answers.Any()) ? "[Ex. " + n + "] - " + answers[randomExercise] + "\n" : string.Empty;
                exercises.RemoveAt(randomExercise);
                if (answers.Any()) answers.RemoveAt(randomExercise);               
                n++;
            }
            finishedTest += "\n" + suggestedAnswerKey;
            Form1.fr.progressBar1.Visible = false;
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
        public static void GenerateAnswerSheet(string test_Name, int test_ExerciseAmount, int test_GroupsAmount, int test_possibleAnswersAmount)
        {
            using (Bitmap bmp = new Bitmap(720, 1280))
            {
                Rectangle innerBorder = new Rectangle(70, 70, 580, 1140);
                Rectangle outerBorder = new Rectangle(1, 1, 719, 1279);
                Rectangle studentData = new Rectangle(70, 1, 580, 70);
                Rectangle testID = new Rectangle(70, 1210, 580, 70);
                Graphics g = Graphics.FromImage(bmp);
                StringFormat sf = new StringFormat();
                Font fn = new Font("Calibri", 20);
                Brush br = Brushes.Black;
                Pen pn = Pens.Black;
                String possibleAnswers = GetPossibleAnswers(test_possibleAnswersAmount);
                sf.Alignment = StringAlignment.Center;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                Bitmap qrcode = new Bitmap(barcodeWriter.Write($"{test_ExerciseAmount}/{test_possibleAnswersAmount}/{test_GroupsAmount}"),83,83);
                g.DrawImage(qrcode, 0,0);
                g.DrawImage(qrcode, 637, 0);
                g.DrawRectangle(Pens.Gray, outerBorder);
                g.DrawRectangle(Pens.Black, testID);
                g.DrawRectangle(Pens.Black, innerBorder);
                g.DrawRectangle(Pens.Black, studentData);
                g.DrawString($"{test_Name}; Test Group:", fn, br, studentData, sf);
                sf.Alignment = StringAlignment.Near;
                g.DrawString("\nName and Class Number: ", fn, br, studentData, sf);
                g.DrawString(possibleAnswers, fn, br, 115, 75);
                int offsetY = 0, offsetRecX = 0, offsetRecY = 0, baseX = 75, baseRecX = 110; // offsetY - the offset for drawing the current Exercise number, offsetRecX/Y - the offset for drawing the rectangles
                for (int i = 1; i <= test_ExerciseAmount; i++)
                {
                    if (i > 44)
                    {
                        if (i == 45) g.DrawString(possibleAnswers, fn, br, 395, 75);
                        baseX = 355;
                        baseRecX = 390;
                    }
                    g.DrawString(i.ToString(), fn, br, baseX, 100 + offsetY);
                    for (int j = 0; j < test_possibleAnswersAmount; j++)
                    {
                        g.DrawRectangle(pn, baseRecX + offsetRecX, 105 + offsetRecY, 30, 20);
                        offsetRecX += 39;
                    }
                    offsetRecX = 0;
                    offsetY = (i == 44) ? 0 : offsetY + 25;
                    offsetRecY = (i == 44) ? 0 : offsetRecY + 25;
                }
                sf.Alignment = StringAlignment.Far;
                // END DRAWING ANSWER SHEET
                g.RotateTransform(30);
                g.Flush();
                bmp.Save("answerSheet.bmp");
            }
        }
        public static string GetPossibleAnswers(int num) 
        {
            int i = 0;
            char currentChar = 'A';
            string possibleAnswers = "";
            while (i<num && currentChar <= 90)
            {
                possibleAnswers += currentChar + "    ";
                currentChar++;
                i++;
            }
            return possibleAnswers;
        }
        public static int Check(Bitmap bmp, string testID, Dictionary<int, char> answerKey)
        {
            bmp = GrayScale(bmp);
            Dictionary<int, char> studentAnswers = new Dictionary<int, char>();
            Bitmap box = new Bitmap(29, 19);
            Graphics g = Graphics.FromImage(box);
            Graphics g2 = Graphics.FromImage(bmp);
            Color pixel = Color.White;
            int offsetRecX = 0, offsetRecY = 0, baseRecX = 110;
            box = bmp.Clone(new Rectangle(baseRecX + offsetRecX, 105 + offsetRecY, 29, 19), box.PixelFormat);           
            int test_ExerciseAmount = Convert.ToInt32(testID.Split(new[] { "/" }, StringSplitOptions.None)[0]);
            int test_possibleAnswersAmount = Convert.ToInt32(testID.Split(new[] { "/" }, StringSplitOptions.None)[1]);
            for (int i = 1; i <= test_ExerciseAmount; i++)
            {
                if (i > 44)
                {
                    if (i == 45)
                        baseRecX = 390;
                }
                bool studentHasAnswered = false;
                for (int j = 1; j <= test_possibleAnswersAmount; j++)
                {
                    g2.DrawRectangle(Pens.Red, new Rectangle(baseRecX + offsetRecX, 105 + offsetRecY, 29, 19));
                    if (searchForMarks(box, pixel))
                    {
                        studentAnswers.Add(i, (char)(j + 64));
                        offsetRecX = 0;
                        studentHasAnswered = true;
                        break;
                    }
                    offsetRecX += 39;
                    box = bmp.Clone(new Rectangle(baseRecX + offsetRecX, 105 + offsetRecY, 29, 19), box.PixelFormat);                  
                }
                if (!studentHasAnswered)
                {
                    studentAnswers.Add(i, '-');
                }
                offsetRecX = 0;
                offsetRecY = (i == 44) ? 0 : offsetRecY + 25;
                box = bmp.Clone(new Rectangle(baseRecX + offsetRecX, 105 + offsetRecY, 29, 19), box.PixelFormat);
            }
            int correctAnswers = 0;
            for (int i = 1; i <= answerKey.Count; i++)
            {
                if (answerKey[i] == studentAnswers[i])
                {
                    correctAnswers++;
                }
            }
            bmp.Save("asdf.bmp");
            g.Flush();
            return correctAnswers;
        }
        public static Bitmap GrayScale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
        public static bool searchForMarks(Bitmap box, Color pixel)
        {
            int foundMarks = 0;
            for (int x = 0; x < box.Width; x++)
            {
                for (int y = 0; y < box.Height; y++)
                {
                    pixel = box.GetPixel(x, y);
                    if (pixel.R <= 100)
                    {
                        foundMarks++;
                        if (foundMarks >= 20)
                        {
                            foundMarks = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
