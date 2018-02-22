using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace English_Test_Generator
{
    class CacheWord
    {
        public static bool Check(string word, string typeOfRequest)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/" + typeOfRequest + @"/" + word + " - " + typeOfRequest.ToLower() + ".etg");
            return File.Exists(path);
        }
        public static void Write(string word, string typeOfRequest, string cache)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/" + typeOfRequest + @"/" + word + " - " + typeOfRequest.ToLower() + ".etg");
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(cache);
            }
        }
        public static string Read(string word, string typeOfRequest, string lexicalCategory)
        {
            string cache = ""; // clear the cache
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/" + typeOfRequest + @"/" + word + " - " + typeOfRequest.ToLower() + ".etg");
            using (StreamReader sr = File.OpenText(path))            
            {
                string currentLine = "";
                bool containsLexicalCategory = false;
                while ((currentLine = sr.ReadLine()) != null) // will loop through all of the lines in the saved file
                {
                    if (lexicalCategory == "")
                    {
                        cache = sr.ReadToEnd();
                        break;
                    }
                    if (containsLexicalCategory)
                    {
                        cache += currentLine + "\n";
                    }
                    if (currentLine == "[" + lexicalCategory.ToUpper() + " - " + typeOfRequest.ToUpper() + "]")
                    {
                        cache += currentLine + "\n";
                        containsLexicalCategory = true;
                    }
                    else
                    {
                        containsLexicalCategory = false;
                    }
                }
                return cache;
            }
        }
    }
}
