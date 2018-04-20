using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using English_Test_Generator;
using Newtonsoft.Json;
using JSON_lib;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using ZXing;
using System.Threading;
using System.Text.RegularExpressions;
using Ghostscript.NET.Rasterizer;
using System.IO;
using System.Collections.Concurrent;

namespace English_Test_Generator
{
    class Utility
    {
        private static Random rng = new Random();
        public static List<T> ShuffleElements<T> (List<T> list)
        {
            List<T> shuffledList = new List<T>(); 
            foreach (var item in list.ToList())
            {
                int randomNumber = rng.Next(list.Count());
                shuffledList.Add(list[randomNumber]);
                list.RemoveAt(randomNumber);
            }
            return shuffledList;
        }
        public static bool HasRequestsLeft(string app_Id, string app_Key)
        {
            string url = "https://od-api.oxforddictionaries.com:443/api/v1/filters"; // URL for the request 
            HttpClient client = new HttpClient(); // creates an HTTP Client
            HttpResponseMessage response = new HttpResponseMessage(); // used to get the API Response            
            client.BaseAddress = new Uri(url); // sets the client address to the specified url
            client.DefaultRequestHeaders.Add("app_id", app_Id); // adds the id to the headers
            client.DefaultRequestHeaders.Add("app_key", app_Key); // adds the key to the headers
            try { response = client.GetAsync(url).Result; } // gets the respone headers   
            catch (Exception) { }
            if (response.StatusCode.ToString() == "Forbidden") { return false; };
            return true;
        }
        public static void GetNewCredentials()
        {
            string apiCredentialsList = "";
            using (WebClient wc = new WebClient())
            {
                try { apiCredentialsList = wc.DownloadString("https://pastebin.com/raw/Pu4ki8eE"); }
                catch (Exception) { MessageBox.Show("Unable to connect to the internet. Restart the program with internet connectivity at least once!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                string[] apiCredentials = apiCredentialsList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                string app_Id = "";
                string app_Key = "";
                for (int i = 0; i < apiCredentials.Length; i++)
                {
                    app_Id = apiCredentials[i].Split(new[] { ":" }, StringSplitOptions.None)[0];
                    app_Key = apiCredentials[i].Split(new[] { ":" }, StringSplitOptions.None)[1];
                    if (HasRequestsLeft(app_Id, app_Key))
                    {
                        TestGeneratorForm.app_Id = app_Id;
                        TestGeneratorForm.app_Key = app_Key;
                        English_Test_Generator.Properties.Settings.Default.app_Id = app_Id;
                        English_Test_Generator.Properties.Settings.Default.app_Key = app_Key;
                        English_Test_Generator.Properties.Settings.Default.Save();
                        return;
                    }
                }
            }
        }
        public static Bitmap RotateBMP(Bitmap bmp, float Ax, float Ay, float Bx, float By)
        {
                float angle = (float)Math.Atan2(Math.Abs(By - Ay), Math.Abs(Bx - Ax));
                PointF centerOld = new PointF((float)bmp.Width / 2, (float)bmp.Height / 2);
                Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height, bmp.PixelFormat);
                newBitmap.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
                using (Graphics g = Graphics.FromImage(newBitmap))
                {
                    Matrix matrix = new Matrix();
                    float angleToRotate = (angle * (180.0f / (float)Math.PI));
                    float fractionalPortion = angleToRotate - (float)Math.Truncate(angleToRotate);
                    float toAdd = fractionalPortion;
                    matrix.RotateAt(angleToRotate, centerOld);
                    g.Transform = matrix;
                    g.DrawImage(bmp, new System.Drawing.Point());
                    //newBitmap.Save("rotatedImage.bmp");
                    return newBitmap;
                }
        }
        public static Bitmap ConvertPDFToPNG(string inputFile)
        {
            using (var rasterizer = new GhostscriptRasterizer()) //create an instance for GhostscriptRasterizer
            {
                rasterizer.Open(inputFile); //opens the PDF file for rasterizing
                var pdf2png = rasterizer.GetPage(100, 100, 1);
                return (Bitmap)pdf2png;
            }
        }
        public static bool ReadQRCode(Bitmap bmp, out ZXing.Result result, int timesRotated)
        {
            BarcodeReader barcodeReader = new BarcodeReader();
            barcodeReader.Options.TryHarder = true;
            var barcodeBitmap = (Bitmap)bmp;
            result = barcodeReader.Decode(barcodeBitmap);
            if(result == null && timesRotated<=3)
            {
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                timesRotated++;
                ReadQRCode(bmp, out result, timesRotated);
            }
            return (result == null) ? false : true;
        }
        public static string GenerateChoices(List<string> choices, string word, out char answer)
        {
            string result = "";
            answer = 'A';
            foreach (var choice in choices)
            {
                if (choice==word)
                {
                    answer = (char)(choices.IndexOf(choice) + 65);
                }
                result +=
                    (char)(choices.IndexOf(choice) + 65) + ") " +
                    choice + "    ";
            }
            return result;
        }
        /// <summary>
        /// Encrypts a given string to base64.
        /// </summary>
        public static string Encrypt(string input)
        {
            string output = "";
            byte[] b = Encoding.ASCII.GetBytes(input);
            output = Convert.ToBase64String(b);
            return output;
        }
        /// <summary>
        ///  Decrypts a given string from base64.
        /// </summary>
        public static string Decrypt(string input)
        {
            string output = "";
            byte[] b;
            try
            {
                b = Convert.FromBase64String(input);
                output = Encoding.ASCII.GetString(b);
            }
            catch (Exception)
            {
                output = "";               
            }
            return output;
        }
        /// <summary>
        /// Checks whether a given word has an entry in the Oxford Dictionary's database.
        /// </summary>
        public static bool IsValidEntry(KeyValuePair<string,string> entry, string test_Type)
        {
            switch (test_Type)
            {
                case "Definitions":
                    string definition = Test.Read(GetAPIResponse.Definitions.get(entry.Value, entry.Key));
                    return (definition.Contains("Couldn't find ") && definition.Contains("ERROR")) ? true:false;
                case "Examples":
                    string example = Regex.Replace(Test.Read(GetAPIResponse.Examples.get(entry.Value, entry.Key)), entry.Key, new string('_', entry.Key.Length), RegexOptions.IgnoreCase);
                    return ((example.Contains("Couldn't find ") && example.Contains("ERROR"))) ? true : false;
                case "Words":
                    return true; // no need to check anything, becuase test type words doesn't request anything from Oxford
                case "Multi-Choices":
                    char answer = 'A'; // stores the correct answer for each exercise
                    string multi_choices = Regex.Replace(Test.Read(GetAPIResponse.Examples.get(entry.Value, entry.Key)), entry.Key, new string('_', entry.Key.Length), RegexOptions.IgnoreCase) + "\n" + GetAPIResponse.Synonyms.Request(entry.Key, out answer);
                    return (multi_choices.Contains("Couldn't find ") && multi_choices.Contains("ERROR")) ? true : false;
            }
            return false;
        }
        /// <summary>
        /// Returns collection of blobs which contain all of the detected answers.
        /// </summary>       
        public static void InitializeAnswerSheet (out ConcurrentBag<string> studentsResults, string filePath)
        {
            Dictionary<int, char> answerKey = new Dictionary<int, char>();
            string testID = "";
            string studentName = Path.GetFileNameWithoutExtension(filePath);
            int Ax, Ay, Bx, By;
            Bitmap bmp = (Bitmap)Bitmap.FromFile(filePath);
            studentsResults = new ConcurrentBag<string>();
            if (!ReadQRCode(bmp, out ZXing.Result barcodeResult, 0))
            {
                MessageBox.Show("Please adjust " + studentName + "'s answer sheet!","Checking failed", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                studentsResults.Add("Couldn't check " + studentName +"'s Answer Sheet.");
                return;
            }
            Ax = (int)barcodeResult.ResultPoints[1].X;
            Ay = (int)barcodeResult.ResultPoints[1].Y;
            Bx = (int)barcodeResult.ResultPoints[2].X;
            By = (int)barcodeResult.ResultPoints[2].Y;
            Graphics g = Graphics.FromImage(bmp);
            bmp = RotateBMP(bmp, Ax, Ay, Bx, By);
            testID = Decrypt(barcodeResult?.Text);
            testID.TrimEnd();
            string[] lines = testID.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i <= lines.Length - 1; i++)
            {
                string[] currentLine = lines[i].Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                answerKey.Add(i, currentLine[1][0]);
            }
            studentsResults.Add((studentName+" has scored: " + Test.Check(bmp, testID, answerKey).ToString() + "/" + answerKey.Count + " points"));
        }
    }
}
