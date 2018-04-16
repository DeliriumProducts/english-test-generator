// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheWord.cs" company="Delirium Products">
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
                sw.WriteLine(cache.Trim()); // writes the cache to the file
            }
        }
        public static void Write(string word, string typeOfRequest, List<string> cache)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/" + typeOfRequest + @"/" + word + " - " + typeOfRequest.ToLower() + ".etg"); // sets the path to the word
            using (StreamWriter sw = File.CreateText(path)) // creates object to write text files with and creates the file for caching
            {
                foreach (var synonym in cache)
                {
                    sw.WriteLine(synonym); // writes the cache to the file
                }
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
                return cache.Trim();
            }
        }
        public static string Read(string word, string typeOfRequest)
        {
            List<string> cache = new List<string>(); // temporary cache
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ETGCachedData/" + typeOfRequest + @"/" + word + " - " + typeOfRequest.ToLower() + ".etg"); // sets the path to the word
            using (StreamReader sr = File.OpenText(path))
            {
                string currentLine = "";
                while ((currentLine = sr.ReadLine()) != null)
                {
                    cache.Add(currentLine);
                }
            }
            return cache;            
        }
    }
}
