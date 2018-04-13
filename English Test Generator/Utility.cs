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

namespace English_Test_Generator
{
    class Utility
    {
        public static bool hasRequestsLeft(string app_Id, string app_Key)
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
        public static void getNewCredentials()
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
                    if (hasRequestsLeft(app_Id, app_Key))
                    {
                        Form1.app_Id = app_Id;
                        Form1.app_Key = app_Key;
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
            if (!(Ax == Bx || Ay == By))
            {
                float angle = (float)Math.Atan2(Math.Abs(By - Ay), Math.Abs(Bx - Ax));
                PointF centerOld = new PointF((float)bmp.Width / 2, (float)bmp.Height / 2);
                Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height, bmp.PixelFormat);
                newBitmap.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
                PointF centerNew = new PointF((float)newBitmap.Width / 2, (float)newBitmap.Height / 2);
                bool isCentered = false;
                if (centerOld.X == centerNew.X) isCentered = true;
                using (Graphics g = Graphics.FromImage(newBitmap))
                {
                    Matrix matrix = new Matrix();
                    float angleToRotate = (angle * (180.0f / (22.0f/ 7.0f)));
                    float fractionalPortion = angleToRotate - (float)Math.Truncate(angleToRotate);
                    float toAdd = (isCentered) ? 0 : fractionalPortion;
                    matrix.RotateAt(((int)angleToRotate)+toAdd, centerOld);
                    g.Transform = matrix;
                    g.DrawImage(bmp, new PointF());
                    newBitmap.Save("rotatedImage.bmp");
                }
            }
            return bmp;
        }
    }
}
