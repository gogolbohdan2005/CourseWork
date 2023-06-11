using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace main.classes
{
    internal class ApiTools
    {
        public static async Task FetchDataAndSaveToFileAsync()
        {
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            long unixTimestamp1 = currentTime.ToUnixTimeSeconds();
            HttpClient client = new HttpClient();

            try
            {
                long unixTimestamp2 = unixTimestamp1 - 86400; // PREVIOUS DAY
                string apiUrl = $"https://api.monobank.ua/personal/statement/0/{unixTimestamp2}/{unixTimestamp1}";
                string apiToken = "ueWsap4QeC-TTxE-IGpRg4_tB9QX8qhFzGE287ySuwig";

                // Set the X-Token header with the API token
                client.DefaultRequestHeaders.Add("X-Token", apiToken);
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Save the JSON response to a file
                    string filePath = Global.jsonPath;
                    File.WriteAllText(filePath, jsonResponse);
                }
                else
                {
                    MessageBox.Show($"API request failed with status code: {response.StatusCode}", "API Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "API Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                client.Dispose();
            }
        }
        public static string GenerateJsonPath()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            folderPath += Global.folderName;
            Directory.CreateDirectory(folderPath);
            string jsonPath = System.IO.Path.Combine(folderPath, Global.jsonFileName);
            return jsonPath;
        }

        public static void InsertFromJsonToTable(string filePath)
        {
            // Read the JSON file
            string json = File.ReadAllText(filePath);
            // Deserialize the JSON array into a C# object
            var jsonArray = JsonConvert.DeserializeObject<dynamic[]>(json);

            foreach (var item in jsonArray)
            {
                string inf = item.amount.ToString();
                int price = int.Parse(inf) / -100;

                if (price > 0)
                {
                    sqlManager.InsertDatabase("Card", item.description.ToString(), price);
                }
            }
        }
    }
}
