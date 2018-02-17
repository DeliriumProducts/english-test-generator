using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using English_Test_Generator;
using Newtonsoft.Json;
using JSON_lib;
namespace GetAPIResponse
{
    enum LexicalCategory
    {
        AllTypes, Adjective, Adverb, Noun, Idiomatic, Verb
    }
                  

    class Definitions
    {
        public static string Request(string lexicalCategory, string word)
        {
            string filter = (lexicalCategory=="") ? "noun,adjective,verb,adverb,idiomatic" : lexicalCategory; // de
            string definitions = ""; // variable to store the result
            string url = "https://od-api.oxforddictionaries.com:443/api/v1/entries/en/" + word + "/lexicalCategory=" + filter +";definitions;regions=" + Form1.region; // defines the URL for the request 
            HttpClient client = new HttpClient(); // creates an HTTP Client
            HttpResponseMessage response; // used to get the API Response            
            client.BaseAddress = new Uri(url); // sets the client address to the specified url
            client.DefaultRequestHeaders.Add("app_id", Form1.app_Id); // adds the id to the headers
            client.DefaultRequestHeaders.Add("app_key", Form1.app_Key); // adds the key to the headers
            response = client.GetAsync(url).Result; // gets the response
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result; // receives the API response              
                var result = JsonConvert.DeserializeObject<GetResponse>(content); // Converts the API response to the format that the program can understand
                for (int i = 0; i < result.Results.First().LexicalEntries.Length; i++) // i = all entries from the API response
                {                
                    for (int j = 0; j < result.Results.First().LexicalEntries[i].Entries.Length ; j++) // j = all senses from the API response
                    {                        
                        for (int k = 0; k < result.Results.First().LexicalEntries[i].Entries[j].Senses.Length; k++) // k = all definitions from the API response 
                        {
                            definitions += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper()+"] "
                            + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Definitions.First() + "\n"; // adds the definition to the variable
                            if (result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses !=null) // checks if there is atleast one subsense in the current sense 
                            {
                                for (int l = 0; l < result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses.Length; l++) // l = all subsense definitions from the API response
                                {
                                    definitions += "[" + result.Results.First().LexicalEntries[i].LexicalCategory.ToUpper() + "] "
                                    + result.Results.First().LexicalEntries[i].Entries[j].Senses[k].Subsenses[l].Definitions.First() + "\n"; // adds the definition to the variable
                                }
                            }
                        }                    
                    }
                }
                return definitions;
            }
            else
            {                
                return "Couldn't establish connection to the server ASDF. Status: " + response.StatusCode; // error while trying to access the API 
            }           
        }
        public static string get(LexicalCategory category, string word)
        {          
            switch (category) // requests the method for the corresponding category
            {
                case LexicalCategory.AllTypes:
                    return Request("",word);                  
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
                default:
                    return "Couldn't find the specified lexical category!";
            }          
        }
        public static string get(string category, string word)
        {
           return get(map.FirstOrDefault(x => x.Value == category).Key, word); 
        }                               
        public static Dictionary<LexicalCategory, string> map =
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
    class Examples
    {
      
    }
}
