using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;
using NewsCollector.BLL.Interfaces;
using NewsGatheringService.DAL.Models;

namespace NewsCollector.BLL.Helpers
{

    public class NewsTextLemmatization : INewsTextLemmatization
    {
        private readonly AppSettings _appSettings;

        public NewsTextLemmatization(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string[] Lemmas { get; set; }

        public async Task<string> RunAsync(string text)
        {
            try
            {
                string lemmasText = "";

                if (string.IsNullOrEmpty(text)) return lemmasText;

                string _text = "\"" + text + "\"";

                var client = new HttpClient();
                
                client.DefaultRequestHeaders.Accept
                    .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, 
                    $"http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey={_appSettings.LemmaKey}");
               
                request.Content = new StringContent("[{\"text\":" + _text + "}]", Encoding.UTF8, "application/json");

                var response = client.SendAsync(request).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    
                    Root[] restoredPerson = await JsonSerializer.DeserializeAsync<Root[]>(data);
                    
                    lemmasText = restoredPerson
                        .Select(r => string.Join(" ", r.annotations.lemma.Select(l => l.value)))
                        .SingleOrDefault();
                }
                
                return lemmasText;
            }
            catch
            {
                throw;
            }

        }
    }
    class Root
    {
        public string text { get; set; }
        
        public Annotations annotations { get; set; }
        
        public class LemmaItem
        {
            public string value { get; set; }
        }
        
        public class Annotations
        {
            public List<LemmaItem> lemma { get; set; }
        }
    }
}
