using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace AlertMe
{
    class PushBullet
    {
        [DataContract]
        public class Note
        {
            public Note(string title, string body)
            {
                this.body = body;
                this.title = title;
                this.type = "note";
            }

            [DataMember]
            private string type;
            [DataMember]
            private string title;
            [DataMember]
            private string body;

            public async Task<bool> Push(string token)
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pushbullet.com/v2/pushes");
                    request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
                    request.Content = new StringContent(this.toJson(), Encoding.UTF8, "application/json");
                    var client = new HttpClient();
                    var response = await client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        Log.Bot.Print("Failed sending pushbullet message");
                        Log.Bot.Print("Status code: " + response.StatusCode.ToString());
                        return false;
                    }
                    else
                        return true;
                }
                else
                {
                    Log.Bot.Print("Pushbullet, missing token.");
                    return false;
                }
            }

            public async Task<bool> Push()
            {
                return await Push(Settings.Current.pushBullet.Token);
            }
            private string toJson()
            {                
                var jsonSerializer = new DataContractJsonSerializer(typeof(Note));
                var memoryStream = new MemoryStream();
                var streamReader = new StreamReader(memoryStream);
                try
                {
                    jsonSerializer.WriteObject(memoryStream, this);
                    memoryStream.Position = 0;
                    return streamReader.ReadToEnd();
                }
                finally
                {
                    streamReader.Close();
                    memoryStream.Close();
                }

            }
        }
    }
}
