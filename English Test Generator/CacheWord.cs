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
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/" + typeOfRequest + @"/" + word + " - " + typeOfRequest.ToLower() + ".etg"); // sets the path to the word
            return File.Exists(path); // checks wheter the file exists or not and returns it
        }
        public static void Write(string word, string typeOfRequest, string cache)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/" + typeOfRequest + @"/" + word + " - " + typeOfRequest.ToLower() + ".etg"); // sets the path to the word
            using (StreamWriter sw = File.CreateText(path)) // creates object to write text files with and creates the file for caching
            {
                sw.WriteLine(cache); // writes the cache to the file
            }
        }
        public static string Read(string word, string typeOfRequest, string lexicalCategory)
        {
            string cache = ""; // temporary cache
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/" + typeOfRequest + @"/" + word + " - " + typeOfRequest.ToLower() + ".etg"); // sets the path to the word
            using (StreamReader sr = File.OpenText(path))            
            {
                string currentLine = "";
                bool containsLexicalCategory = false;
                while ((currentLine = sr.ReadLine()) != null) // loop algorithm to get only the designated lexicalCategory from the cached files
                {
                    if (lexicalCategory == "")
                    {
                        cache = currentLine + "\n" + sr.ReadToEnd();
                        break;
                    }
                    if (containsLexicalCategory) // will
                    {
                        cache += currentLine + "\n";
                    }
                    if (currentLine == "[" + lexicalCategory.ToUpper() + " - " + typeOfRequest.ToUpper() + "]") // checks if the current line contains that specific string (ETG's specific synatx)
                    {
                        cache += currentLine + "\n"; // adds the current line to the temporary cache
                        containsLexicalCategory = true; // bool variable to also get the line AFTER the current one
                    }
                    else 
                    {
                        containsLexicalCategory = false; // resets the variable to prevent conflicts
                    }
                }
                return cache;
            }
        }
    }
}
