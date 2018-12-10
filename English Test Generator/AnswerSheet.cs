using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;

namespace English_Test_Generator
{
    class AnswerSheet
    {
        public static void Generate(string testName, int testExerciseAmount, int testGroupsAmount, int testPossibleAnswersAmount, string testAnswerKey)
        {
            using (Bitmap bmp = new Bitmap(720, 1280))
            {
                Rectangle innerBorder = new Rectangle(70, 70, 580, 1140);
                Rectangle outerBorder = new Rectangle(0, 0, 720, 1280);
                Rectangle studentData = new Rectangle(70, 1, 580, 70);
                Graphics g = Graphics.FromImage(bmp);
                StringFormat sf = new StringFormat();
                Font fn = new Font("Calibri", 20);
                Brush br = Brushes.Black;
                Pen pn = Pens.Black;
                String possibleAnswers = GetPossibleAnswers(testPossibleAnswersAmount);
                sf.Alignment = StringAlignment.Center;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = 150,
                        Height = 150
                    }
                };
                Bitmap qrcode = new Bitmap(barcodeWriter.Write(Utility.Encrypt($"{testExerciseAmount}/{testPossibleAnswersAmount}/{testGroupsAmount}\n{testAnswerKey}")));
                g.DrawImage(qrcode, bmp.Width - qrcode.Width, bmp.Height - qrcode.Height);
                g.DrawRectangle(Pens.Black, studentData);
                g.DrawRectangle(new Pen(Brushes.Gray, 10), outerBorder);
                g.DrawString($"{testName}; Test Group:", fn, br, studentData, sf);
                sf.Alignment = StringAlignment.Near;
                g.DrawString("\nName and Class Number: ", fn, br, studentData, sf);
                int offsetY = 0, offsetRecX = 0, offsetRecY = 0, baseX = 0, baseRecX = (bmp.Width / 2) - ((22 * testPossibleAnswersAmount + 18 * (testPossibleAnswersAmount - 1)) / 2); // offsetY - the offset for drawing the current Exercise number, offsetRecX/Y - the offset for drawing the rectangles
                baseX = baseRecX - 36;
                g.DrawString(possibleAnswers, fn, br, baseRecX, 75);
                for (int i = 1; i <= testExerciseAmount; i++)
                {
                    g.DrawString(i.ToString(), fn, br, baseX, 100 + offsetY);
                    for (int j = 0; j < testPossibleAnswersAmount; j++)
                    {
                        g.DrawEllipse(pn, baseRecX + offsetRecX, 105 + offsetRecY, 22, 22);
                        offsetRecX += 39;
                    }
                    offsetRecX = 0;
                    offsetY += 25;
                    offsetRecY += 25;
                }
                sf.Alignment = StringAlignment.Far;
                // END DRAWING ANSWER SHEET
                g.RotateTransform(30);
                g.Flush();
                bmp.Save($"{testName}.bmp");
            }
        }
        private static string GetPossibleAnswers(int num)
        {
            int i = 0;
            char currentChar = 'A';
            string possibleAnswers = "";
            while (i < num && currentChar <= 90)
            {
                possibleAnswers += currentChar + "    ";
                currentChar++;
                i++;
            }
            return possibleAnswers;
        }
        public static int Check(Bitmap bmp, string testID, Dictionary<int, char> answerKey)
        {
            Dictionary<int, char> studentAnswers = new Dictionary<int, char>();
            int testExerciseAmount = Convert.ToInt32(testID.Split(new[] { "/" }, StringSplitOptions.None)[0]);
            int testPossibleAnswersAmount = Convert.ToInt32(testID.Split(new[] { "/" }, StringSplitOptions.None)[1]);
            int currentExercise = 1;
            int currentLetter = 1;
            Blob[] blobs = Blobs(bmp);
            bool studentHasAnswered = false;
            for (int i = 0; i < blobs.Length; i++)
            {
                if (blobs[i].Fullness >= 0.30)
                {
                    studentAnswers.Add(currentExercise, (char)(currentLetter + 64));
                    studentHasAnswered = true;
                    i += testPossibleAnswersAmount - currentLetter; // offset blobs to next exercise
                    currentExercise++;
                    currentLetter = 1;
                    continue;
                }
                if (!studentHasAnswered && currentLetter == testPossibleAnswersAmount)
                {
                    studentAnswers.Add(currentExercise, '-');
                    studentHasAnswered = true;
                    currentExercise++;
                    currentLetter = 1;
                    continue;
                }
                studentHasAnswered = false;
                currentLetter++;
            }

            //int j = 0;
            //int formula = (test_possibleAnswersAmount - 1) * (test_ExerciseAmount) - 1;
            //while (j < blobs.Length)
            //{

            //    if (blobs[j].Fullness >= 0.30)
            //    {
            //        studentAnswers.Add(currentExercise, (char)(currentLetter + 64));
            //        studentHasAnswered = true;
            //        currentExercise++;
            //        currentLetter = 1;
            //        j = currentExercise-1;
            //        continue;
            //    }
            //    if (!studentHasAnswered && currentLetter == test_possibleAnswersAmount)
            //    {
            //        studentAnswers.Add(currentExercise, '-');
            //        studentHasAnswered = true;
            //        currentExercise++;
            //        currentLetter = 1;
            //        j = currentExercise;
            //        continue;
            //    }
            //    if (j - formula == currentExercise + 1)
            //    {
            //        currentExercise++;
            //        currentLetter = 1;
            //        j -= formula;
            //    }
            //    j += test_ExerciseAmount;
            //    studentHasAnswered = false;
            //    currentLetter++;
            //}
            int correctAnswers = 0;
            for (int i = 1; i <= answerKey.Count; i++)
            {
                if (answerKey.Count == studentAnswers.Count && answerKey[i] == studentAnswers[i])
                {
                    correctAnswers++;
                }
            }
            bmp.Dispose();
            return correctAnswers;
        }
        private static Blob[] Blobs(Bitmap image)
        {
            const float baseArea = 921600.0f; // stores the base area of the answer sheet
            BlobCounter blobCounter = new BlobCounter 
            {
                FilterBlobs = true,
                MinHeight = 1280,
                MinWidth = 720
            };
            blobCounter.ProcessImage(PreProcess(image)); // prepares and processes the image for the blob scanning
            Blob[] blobs = blobCounter.GetObjectsInformation(); // gets all the objects
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            Graphics g = Graphics.FromImage(image);
            Pen redPen = new Pen(Color.Red, 2);
            float k = 1.0f;
            foreach (var blob in blobs) // finds the answer sheet in the scanned image (largest scanned rectangle) and calculates the multiplier (k) for finding the answers
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blob);
                if (shapeChecker.IsQuadrilateral(edgePoints, out List<IntPoint> cornerPoints))
                {
                    if (shapeChecker.CheckPolygonSubType(cornerPoints) == PolygonSubType.Rectangle)
                    {
                        List<System.Drawing.Point> points = new List<System.Drawing.Point>();
                        foreach (var point in cornerPoints)
                        {
                            points.Add(new System.Drawing.Point(point.X, point.Y));
                        }
                        System.Drawing.Point min = new System.Drawing.Point(image.Width, image.Height);
                        System.Drawing.Point max = new System.Drawing.Point(0, 0);
                        List<System.Drawing.Point> pl = new List<System.Drawing.Point>();
                        pl = points.OrderBy(p => p.X).ToList();
                        if (pl[0].Y <= pl[1].Y)
                        {
                            min = pl[0];
                        }
                        else if (pl[0].Y >= pl[1].Y)
                        {
                            min = pl[1];
                        }
                        if (pl[2].Y >= pl[3].Y)
                        {
                            max = pl[2];
                        }
                        else if (pl[2].Y <= pl[3].Y)
                        {
                            max = pl[3];
                        }
                        pl.Remove(min);
                        pl.Remove(max);
                        double width = Math.Sqrt(Math.Pow(pl[1].X - max.X, 2) + Math.Pow(pl[1].Y - max.Y, 2));
                        double height = Math.Sqrt(Math.Pow(pl[0].X - max.X, 2) + Math.Pow(pl[0].Y - max.Y, 2));
                        k = (float)(width * height) / baseArea;
                    }
                }
            }
            k = (float)Math.Round(Math.Sqrt(k)); 
            shapeChecker.RelativeDistortionLimit = 0.05f;
            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 21 * (int)k;
            blobCounter.MinWidth = 21 * (int)k;
            blobCounter.ProcessImage(PreProcess(image));
            blobs = blobCounter.GetObjectsInformation();
            List<Blob> circleBlobs = new List<Blob>();
            int i = 0;
            foreach (var blob in blobs) // finds the answers using the aforementioned multiplier (k)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blob);
                if (shapeChecker.IsCircle(edgePoints, out AForge.Point center, out float radius) || (shapeChecker.CheckShapeType(edgePoints) == ShapeType.Circle))
                {
                    g.DrawEllipse(new Pen(Color.FromArgb(255, i, i, i), 2.0f),
                       (int)(center.X - radius),
                       (int)(center.Y - radius),
                       (int)(radius * 2),
                       (int)(radius * 2));
                    circleBlobs.Add(blob);
                }
                if (i + 10 > 255)
                {
                    i = 0;
                }
                i += 10;
            }
            redPen.Dispose();
            g.Dispose();
            return circleBlobs.ToArray();
        }
        private static Bitmap PreProcess(Bitmap bmp) 
        {
            Grayscale gfilter = new Grayscale(0.2125, 0.7154, 0.0721);
            Invert ifilter = new Invert();
            BradleyLocalThresholding thfilter = new BradleyLocalThresholding();
            bmp = gfilter.Apply(bmp);
            thfilter.ApplyInPlace(bmp);
            ifilter.ApplyInPlace(bmp);
            return bmp;
        }
        public static string GenerateChoices(List<string> choices, string word, out char answer)
        {
            string result = "";
            answer = 'A';
            foreach (var choice in choices)
            {
                if (choice == word)
                {
                    answer = (char)(choices.IndexOf(choice) + 65);
                }
                result +=
                    (char)(choices.IndexOf(choice) + 65) + ") " +
                    choice + "    ";
            }
            return result;
        }
        public static void Initialize(out ConcurrentBag<string> studentsResults, string filePath)
        {
            Dictionary<int, char> answerKey = new Dictionary<int, char>();
            string testID = "";
            string studentName = Path.GetFileNameWithoutExtension(filePath);
            int Ax, Ay, Bx, By;
            Bitmap bmp = (Bitmap)Bitmap.FromFile(filePath);
            studentsResults = new ConcurrentBag<string>();
            if (!Utility.ReadQRCode(bmp, out ZXing.Result barcodeResult, 0))
            {
                MessageBox.Show("Please adjust " + studentName + "'s answer sheet!", "Checking failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                studentsResults.Add("Couldn't check " + studentName + "'s Answer Sheet.");
                return;
            }
            Ax = (int)barcodeResult.ResultPoints[1].X;
            Ay = (int)barcodeResult.ResultPoints[1].Y;
            Bx = (int)barcodeResult.ResultPoints[2].X;
            By = (int)barcodeResult.ResultPoints[2].Y;
            Graphics g = Graphics.FromImage(bmp);
            bmp = Utility.RotateBMP(bmp, Ax, Ay, Bx, By);
            testID = Utility.Decrypt(barcodeResult?.Text);
            testID.TrimEnd();
            string[] lines = testID.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i <= lines.Length - 1; i++)
            {
                string[] currentLine = lines[i].Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                answerKey.Add(i, currentLine[1][0]);
            }
            studentsResults.Add((studentName + " has scored: " + Check(bmp, testID, answerKey).ToString() + "/" + answerKey.Count + " points"));
        }
    }
}
