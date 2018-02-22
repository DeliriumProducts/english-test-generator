using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JSON_lib;
using System.Windows.Forms;

namespace English_Test_Generator
{
    class SearchWord
    {
        public static string GetCorrectWord(string word_id, string region)
        {
            if (CacheWord.Check(word_id, "Definitions") || CacheWord.Check(word_id, "Examples"))
            {
                return word_id;
            }
            String url = "https://od-api.oxforddictionaries.com:443/api/v1/search/en"  + "?q=" + word_id + "&prefix=false&regions="+region; // URL for the request 
            HttpClient client = new HttpClient(); // creates an HTTP Client
            HttpResponseMessage response; // used to get the API Response            
            client.BaseAddress = new Uri(url); // sets the client address to the specified url
            client.DefaultRequestHeaders.Add("app_id", Form1.app_Id); // adds the id to the headers
            client.DefaultRequestHeaders.Add("app_key", Form1.app_Key); // adds the key to the headers
            if (Form1.word_prev == word_id) return Form1.word_id; // exits the method if the entered word by the user mathes the previous word so no unnecessary requests are made
            response = client.GetAsync(url).Result; // gets the respone headers
            if (response.IsSuccessStatusCode) // checks if the response code is equal to 200
            {     
                string content = response.Content.ReadAsStringAsync().Result; // receives the API response              
                var result = JsonConvert.DeserializeObject<GetResponse>(content); // Converts the API response to the format that the program can understand
                word_id = (result.Results == null || result.Results.Length == 0) ? "Word not found" : result.Results.First().Word; // gets the first entry of the searched word    
                if (word_id != Form1.word_id && word_id != "Word not found" && result.Results.Length>=1) // checks if the first result doesn't match the user's word nor is equal to "Word not found" and if there's more than one result   
                {                                                          
                    for (int i = 0; i < result.Results.Length; i++) // starts going through each result
                    {
                        if (Form1.word_id.ToLower()==result.Results[i].Word) // check if the current result matches the user's word
                        {                          
                            return Form1.word_id; // returns the user's word
                        }
                        Form1.fr.comboBox3.Items.Add(result.Results[i].Word); // starts filling comboBox3's items with the results
                    }
                    Form1.fr.comboBox3.Visible = true; // makes comboBox3 visible
                    Form1.fr.comboBox3.Text = "";
                    MessageBox.Show("Multiple results found! Please select one from the dropdown menu!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information); // Alerts the user that there's more than one result                
                    return null; // returns null so that the textbox is empty
                }                    
                return word_id; // returns the first 
            }
            else // if the response code is different than 200
            {
                return "Couldn't establish connection to the server. Status: " + response.StatusCode; // error while trying to access the API 
            }           
        }
    }
}
