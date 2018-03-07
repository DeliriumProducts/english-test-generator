// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAPIResponse.cs" company="Delirium Products">
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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using English_Test_Generator;
using Newtonsoft.Json;
using JSON_lib;
using System.Windows.Forms;
using System.Net;

namespace GetAPIResponse
{
    enum LexicalCategory
    {
        AllTypes, Adjective, Adverb, Noun, Idiomatic, Verb, Residual
    }                  
    class Definitions
    {
        public static string Request(string lexicalCategory, string word)
        {
            string cache = "";
            if (CacheWord.Check(word, "Definitions"))
            {
                return CacheWord.Read(word, "Definitions", lexicalCategory);
            }
            string definitions = ""; // variable to store the result
            string url = "https://od-api.oxforddictionaries.com:443/api/v1/entries/en/" + word + "/definitions;regions=" + Form1.region; // URL for the request 
            HttpClient client = new HttpClient(); // creates an HTTP Client
            HttpResponseMessage response = new HttpResponseMessage(); // used to get the API Response            
            client.BaseAddress = new Uri(url); // sets the client address to the specified url
            client.DefaultRequestHeaders.Add("app_id", Form1.app_Id); // adds the id to the headers
            client.DefaultRequestHeaders.Add("app_key", Form1.app_Key); // adds the key to the headers
            try { response = client.GetAsync(url).Result; } // gets the respone headers   
            catch (Exception) { MessageBox.Show("Unable to connect to the internet. Restart the program with internet connectivity at least once!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }      
            if (response.IsSuccessStatusCode) // checks if the response code is equal to 200
                {
                string content = response.Content.ReadAsStringAsync().Result; // receives the API response              
                var result = JsonConvert.DeserializeObject<GetResponse>(content); // Converts the API response to the format that the program can understand
                for (int i = 0; i < result.Results.First().LexicalEntries.Length; i++) // i = all entries from the API response
                {
                    for (int j = 0; j < result.Results.First().LexicalEntries[i].Entries.Length; j++) // j = all senses from the API response
                    {
                        for (int k = 0; k < result.Results.First().LexicalEntries[i].Entries[j].Senses.Length; k++) // k = all definitions from the API response 
                        {
                            for (int l = 0; result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Definitions != null && l < result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Definitions.Length; l++)
                            {
                                if (result.Results.First().LexicalEntries[i].LexicalCategory.ToLower() == lexicalCategory || lexicalCategory == "") // checks if the current lexicalCategory matches the one designated by the user
                                {
                                    definitions += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + " - DEFINITIONS]\n"
                                    + char.ToUpper(result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Definitions[l][0]) + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Definitions[l].Substring(1) + " \n"; // adds the definition to the variable                               
                                }
                                cache += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + " - DEFINITIONS]\n"
                                    + char.ToUpper(result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Definitions[l][0]) + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Definitions[l].Substring(1) + " \n";
                            }
                            if (result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses != null) // checks if there is at least one subsense in the current sense 
                            {
                                for (int l = 0; l < result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses.Length; l++) // l = all subsense definitions from the API response
                                {
                                    if (result.Results.First().LexicalEntries[i].LexicalCategory.ToLower() == lexicalCategory || lexicalCategory == "") // checks if the current lexicalCategory matches the one designated by the user
                                    {
                                        definitions += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + " - DEFINITIONS]\n"
                                        + char.ToUpper(result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Definitions.First()[0]) + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Definitions.First().Substring(1) + " \n"; // adds the definition to the variable
                                    }
                                    cache += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + " - DEFINITIONS]\n"
                                        + char.ToUpper(result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Definitions.First()[0]) + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Definitions.First().Substring(1) + " \n";
                                }
                            }
                        }
                    }
                }
                CacheWord.Write(word, "Definitions", cache);
                return definitions.Trim(); // returns the result 
            }
            else // if the response code is different than 200
            {   if (response.StatusCode.ToString() == "Forbidden") { Utility.getNewCredentials(); get(lexicalCategory, word); }             
                return "ERROR \nCouldn't find " + word + " Status: " + response.StatusCode; // error while trying to access the API 
            }           
        }
        public static string get(LexicalCategory category, string word)
        {                    
            switch (category) // requests the method for the corresponding category
            {
                case LexicalCategory.AllTypes:
                    return Request("", word);
                case LexicalCategory.Adjective:
                    return Request("adjective", word);                                    
                case LexicalCategory.Adverb:
                    return Request("adverb", word);                
                case LexicalCategory.Noun:
                    return Request("noun", word);                  
                case LexicalCategory.Idiomatic:
                    return Request("idiomatic", word);
                case LexicalCategory.Verb:
                    return  Request("verb", word);
                case LexicalCategory.Residual:
                    return Request("residual", word);
                default:
                    return "Couldn't find the specified lexical category!";
            }          
        }
        public static string get(string category, string word)
        {
            return get(map.FirstOrDefault(x => x.Value == category).Key, word); // uses the map to call the get method with the proper arguments
        }                               
        public static Dictionary<LexicalCategory, string> map = // dictionary used as a "map" for each type
            new Dictionary<LexicalCategory, string>
            {
                { LexicalCategory.AllTypes, "All Types"},
                { LexicalCategory.Adjective, "adjective"},
                { LexicalCategory.Adverb, "adverb"},
                { LexicalCategory.Noun, "noun"},
                { LexicalCategory.Idiomatic, "idiomatic"},
                { LexicalCategory.Verb, "verb"},
                { LexicalCategory.Residual, "residual"}
            };
    }
    class Examples
    {
        public static string Request(string lexicalCategory, string word)
        {
            string cache = ""; 
            if (CacheWord.Check(word, "Examples"))
            {
                return CacheWord.Read(word, "Examples", lexicalCategory);
            }
            string examples = "";
            string url = "https://od-api.oxforddictionaries.com:443/api/v1/entries/en/" + word + "/examples;regions=" + Form1.region; // URL for the request 
            HttpClient client = new HttpClient(); // creates an HTTP Client
            HttpResponseMessage response = new HttpResponseMessage(); // used to get the API Response            
            client.BaseAddress = new Uri(url); // sets the client address to the specified url
            client.DefaultRequestHeaders.Add("app_id", Form1.app_Id); // adds the id to the headers
            client.DefaultRequestHeaders.Add("app_key", Form1.app_Key); // adds the key to the headers
            try { response = client.GetAsync(url).Result; }// gets the respone headers   
            catch (Exception) { MessageBox.Show("Unable to connect to the internet. Restart the program with internet connectivity at least once!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result; // receives the API response              
                var result = JsonConvert.DeserializeObject<GetResponse>(content); // Converts the API response to the format that the program can understand     
                for (int i = 0; i < result.Results.First().LexicalEntries.Length; i++) // i = all entries from the API response
                {
                    for (int j = 0; j < result.Results.First().LexicalEntries[i].Entries.Length; j++) // j = all senses from the API response
                    {
                        for (int k = 0; k < result.Results.First().LexicalEntries[i].Entries[j].Senses.Length; k++) // k = all examples from the API response 
                        {
                            for (int l = 0; result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Examples != null && l < result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Examples.Length ; l++) // l = all text in the current example from the API response
                            {
                                if (result.Results.First().LexicalEntries[i].LexicalCategory.ToLower() == lexicalCategory || lexicalCategory == "") // checks if the current lexicalCategory matches the one designated by the user
                                {
                                    examples += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + " - EXAMPLES]\n"
                                        + char.ToUpper(result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Examples[l].Text[0]) + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Examples[l].Text.Substring(1) + ".\n"; // adds the example to the variable
                                }
                                cache += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + " - EXAMPLES]\n"
                                        + char.ToUpper(result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Examples[l].Text[0]) + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Examples[l].Text.Substring(1) + ".\n";
                            }
                            if (result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses != null) // checks if there is at least one subsense in the current sense 
                            {
                                for (int l = 0; l < result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses.Length; l++) // l = all subsense definitions from the API response
                                {
                                    for (int m = 0; m < result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Examples.Length;  m++) // m = all text in the current example from the API response
                                    {
                                        if (result.Results.First().LexicalEntries[i].LexicalCategory.ToLower() == lexicalCategory || lexicalCategory == "") // checks if the current lexicalCategory matches the one designated by the user
                                        {
                                            examples += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + " - EXAMPLES]\n"
                                                + char.ToUpper(result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Examples[m].Text[0]) + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Examples[m].Text.Substring(1) + ".\n"; // adds the example to the variable 
                                        }
                                        cache += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + " - EXAMPLES]\n"
                                                + char.ToUpper(result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Examples[m].Text[0]) + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Examples[m].Text.Substring(1) + ".\n";
                                    }
                                }
                            }
                        }
                    }
                }
                 CacheWord.Write(word, "Examples", cache);
                 return examples.Trim();
            }
            else // if the response code is different than 200
            {
                 if (response.StatusCode.ToString() == "Forbidden") { Utility.getNewCredentials(); get(lexicalCategory, word); }
                 return "ERROR \nCouldn't find " + word + " Status: " + response.StatusCode; // error while trying to access the API 
            }
        }
        public static string get(LexicalCategory category, string word)
        {
            switch (category) // requests the method for the corresponding category
            {
                case LexicalCategory.AllTypes:
                    return Request("", word);
                case LexicalCategory.Adjective:
                    return Request("adjective", word);
                case LexicalCategory.Adverb:
                    return Request("adverb", word);
                case LexicalCategory.Noun:
                    return Request("noun", word);
                case LexicalCategory.Idiomatic:
                    return Request("idiomatic", word);
                case LexicalCategory.Verb:
                    return Request("verb", word);
                default:
                    return "Couldn't find the specified lexical category!";
            }
        }
        public static string get(string category, string word)
        {
            return get(map.FirstOrDefault(x => x.Value == category).Key, word); // uses the map to call the get method with the proper arguments
        }
        public static Dictionary<LexicalCategory, string> map = // dictionary used as a "map" for each type
            new Dictionary<LexicalCategory, string>
            {
                { LexicalCategory.AllTypes, "All Types"},
                { LexicalCategory.Adjective, "adjective"},
                { LexicalCategory.Adverb, "adverb"},
                { LexicalCategory.Noun, "noun"},
                { LexicalCategory.Idiomatic, "idiomatic"},
                { LexicalCategory.Verb, "verb"},
            };
    }
    class Utility
    {
        public static bool hasRequestsLeft(string app_Id, string app_Key)
        {
            string url = "https://od-api.oxforddictionaries.com:443/api/v1/filters";// URL for the request 
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
            using (WebClient wc = new WebClient())
            {
                string apiCredentialsList = wc.DownloadString("https://pastebin.com/raw/Pu4ki8eE");
                string[] apiCredentials = apiCredentialsList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                string app_Id = "";
                string app_Key = "";
                for (int i = 0; i < apiCredentials.Length; i++)
                {
                    app_Id = apiCredentials[i].Split(new[] { ":"}, StringSplitOptions.None)[0];
                    app_Key = apiCredentials[i].Split(new[] { ":" }, StringSplitOptions.None)[1];
                    if(hasRequestsLeft(app_Id,app_Key))
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
    }
}
