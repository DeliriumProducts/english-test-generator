// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchWord.cs" company="Delirium Products">
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
            HttpResponseMessage response = new HttpResponseMessage(); // used to get the API Response            
            client.BaseAddress = new Uri(url); // sets the client address to the specified url
            client.DefaultRequestHeaders.Add("app_id", TestGeneratorForm.appId); // adds the id to the headers
            client.DefaultRequestHeaders.Add("app_key", TestGeneratorForm.appKey); // adds the key to the headers
            if (TestGeneratorForm.wordPrev == word_id) return TestGeneratorForm.wordId; // exits the method if the entered word by the user mathes the previous word so no unnecessary requests are made
            try {response = client.GetAsync(url).Result;}// gets the respone headers   
            catch (Exception) {MessageBox.Show("Unable to connect to the internet. Restart the program with internet connectivity at least once!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);}              
            if (response.IsSuccessStatusCode) // checks if the response code is equal to 200
            {     
                string content = response.Content.ReadAsStringAsync().Result; // receives the API response              
                var result = JsonConvert.DeserializeObject<GetResponse>(content); // Converts the API response to the format that the program can understand
                word_id = (result.Results == null || result.Results.Length == 0) ? "Word not found" : result.Results.First().Word; // gets the first entry of the searched word    
                if (word_id != TestGeneratorForm.wordId && word_id != "Word not found" && result.Results.Length>=1) // checks if the first result doesn't match the user's word nor is equal to "Word not found" and if there's more than one result   
                {                                                          
                    for (int i = 0; i < result.Results.Length; i++) // starts going through each result
                    {
                        if (TestGeneratorForm.wordId.ToLower() == result.Results[i].Word) // check if the current result matches the user's word
                        {                          
                            return TestGeneratorForm.wordId; // returns the user's word
                        }
                        TestGeneratorForm.fr.comboBox3.Items.Add(result.Results[i].Word); // starts filling comboBox3's items with the results
                    }
                    TestGeneratorForm.fr.comboBox3.Visible = true; // makes comboBox3 visible
                    TestGeneratorForm.fr.comboBox3.Text = "";
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
