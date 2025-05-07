using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string folderPath = "Users";
        DirectoryInfo directory = new DirectoryInfo(folderPath);
        if (!directory.Exists)
            directory.Create();

        string filePath = Path.Combine(folderPath, "users.json");
        FileInfo file = new FileInfo(filePath);
        if (!file.Exists)
            using (file.Create()) { }

        using (HttpClient client = new HttpClient())
        {
            try
            {
                string url = "https://jsonplaceholder.typicode.com/users";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonData = await response.Content.ReadAsStringAsync();

                var users = JsonSerializer.Deserialize<List<User>>(jsonData);

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    string formattedJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                    writer.Write(formattedJson);
                    Console.WriteLine("Data users.json faylına yazıldı.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Xəta baş verdi: {ex.Message}");
            }
        }
    }
}

