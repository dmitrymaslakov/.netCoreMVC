using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;


namespace NewsCollector.BLL.Helpers
{
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

    public class NewsTextLemmatization
    {
        private const string _lemmaKey = "4fa5baf75687bcda39e604126aa4b9b70fdcf517";
        //private readonly string _text;

        /*public NewsTextLemmatization(string text)
        {
            _text = "\"" + text + "\"";
        }*/


        public string[] Lemmas { get; set; }
        /// <summary>
        /// Starts lemmarization of news text
        /// </summary>
        /// <param name="text">news text</param>
        /// <returns>Returns lemmatized news text</returns>
        public static async Task<string> RunAsync(string text)
        {
            string lemmasText = "";
            if (string.IsNullOrEmpty(text)) return lemmasText;
            string _text = "\"" + text + "\"";
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            var request = new HttpRequestMessage(HttpMethod.Post, $"http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey={_lemmaKey}");
            request.Content = new StringContent("[{\"text\":" + _text + "}]",
                                                Encoding.UTF8,
                                                    "application/json");//CONTENT-TYPE header

            var response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStreamAsync();
                Root[] restoredPerson = await JsonSerializer.DeserializeAsync<Root[]>(data);
                lemmasText = restoredPerson
                    .Select(r => string.Join(" ", r.annotations.lemma.Select(l => l.value)))
                    .SingleOrDefault();
                //restoredPerson.SelectMany(r => r.annotations.lemma.Select(l => l.value))
            }
            return lemmasText;
        }
    }
}
