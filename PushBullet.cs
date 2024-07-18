
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using JsonTextWriter = Newtonsoft.Json.JsonTextWriter;
using NullValueHandling = Newtonsoft.Json.NullValueHandling;

#pragma warning disable CA1416

namespace AlertMe
{
    class PushBullet
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public class Note
        {
            public string type => "note";
            public string title;
            public string body;


            public Note(string title, string body)
            {
                this.title = title;
                this.body = body;
            }

            public async Task<bool> Push(string token)
            {
                if (string.IsNullOrEmpty(token))
                {
                    Log.Bot.Print("Pushbullet, missing token.");
                    return false;
                }


                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pushbullet.com/v2/pushes");
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Content = new StringContent(this.ToJson(), Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Log.Bot.Print("Failed sending pushbullet message");
                    Log.Bot.Print("Status code: " + response.StatusCode.ToString());
                    return false;
                }
                else
                    return true;


            }

            public async Task<bool> Push()
            {
                return await Push(Settings.Current.pushBullet.Token);
            }
            public string ToJson()
            {
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;


                using var memoryStream = new MemoryStream();
                using var streamWriter = new StreamWriter(memoryStream);
                using var jsonWriter = new JsonTextWriter(streamWriter);
                serializer.Serialize(jsonWriter, this);
                jsonWriter.Flush();

                memoryStream.Position = 0;
                using var streamReader = new StreamReader(memoryStream);

                return streamReader.ReadToEnd();
            }
        }
    }
}
