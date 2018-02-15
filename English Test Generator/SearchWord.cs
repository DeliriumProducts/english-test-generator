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
        public static string GetCorrectWord(string word_id, string language)
        { 
            String url = "https://od-api.oxforddictionaries.com:443/api/v1/search/en"  + "?q=" + word_id + "&prefix=false&regions="+Properties.Settings.Default.userRegion;
            HttpClient client = new HttpClient(); // creates an HTTP Client
            HttpResponseMessage response; // used to get the API Response            
            client.BaseAddress = new Uri(url); // sets the client address to the specified url
            client.DefaultRequestHeaders.Add("app_id", Form1.app_Id); // adds the id to the headers
            client.DefaultRequestHeaders.Add("app_key", Form1.app_Key); // adds the key to the headers
            if (Form1.word_prev == word_id) return Form1.word_id; // exits the method if the entered word by the user mathes the previous word so no unnecessary requests are made
            response = client.GetAsync(url).Result; // gets the respone headers
            if (response.IsSuccessStatusCode)
            {               
                string content = response.Content.ReadAsStringAsync().Result; // receives the API response              
                var result = JsonConvert.DeserializeObject<GetResponse>(content); // Converts the API response to the format that the program can understand
                word_id = (result.Results == null || result.Results.Length == 0) ? "Word not found" : result.Results.First().Word; // gets the first entry of the searched word    
                if (word_id != Form1.word_id && word_id != "Word not found" && result.Results.Length>=1)
                {                  
                    MessageBox.Show("Multiple results found! Please select one from the dropdown menu!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1.fr.comboBox3.Visible = true;
                    for (int i = 0; i < result.Results.Length; i++)
                    {               
                        Form1.fr.comboBox3.Items.Add(result.Results[i].Word);
                    }
                    return null; // returns null so that the textbox is empty
                }                    
                return word_id; // returns it
            }
            else
            {
                return "Couldn't establish connection to the server. Status: " + response.StatusCode; // error while trying to access the API 
            }           
        }
    }
}
