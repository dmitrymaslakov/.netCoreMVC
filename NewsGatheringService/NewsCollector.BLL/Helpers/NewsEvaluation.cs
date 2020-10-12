using Microsoft.Extensions.Options;
using NewsCollector.BLL.Interfaces;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Helpers
{
    public class NewsEvaluation : INewsEvaluation
    {
        private readonly Dictionary<string, string> _affin;
        private readonly AppSettings _appSettings;
        private readonly INewsTextLemmatization _newsTextLemmatization;

        public NewsEvaluation(IOptions<AppSettings> appSettings, INewsTextLemmatization newsTextLemmatization)
        {
            _appSettings = appSettings.Value;

            using (var r = new StreamReader(_appSettings.AFINN_ruFilePath))
            {
                string json = r.ReadToEnd();
                _affin = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                r.Close();
            }
            
            _newsTextLemmatization = newsTextLemmatization;
        }

        public async Task<int> EvaluateNewsAsync(News news)
        {
            try
            {
                var bodyLemma = await _newsTextLemmatization.RunAsync(news.NewsStructure.Body);
                
                var bodyValue = evaluateLemma(bodyLemma);
                
                return bodyValue;
            }
            catch
            {
                throw;
            }
        }

        private int evaluateLemma(string lemma)
        {
            try
            {
                if (string.IsNullOrEmpty(lemma)) return 0;
                
                var marks =
                     _affin
                     .Where(kv => !string.IsNullOrEmpty(kv.Key))
                     .Select(kv =>
                     {
                         var b = Regex.IsMatch(lemma, "\\b" + kv.Key + "\\b");
                         return b ? int.Parse(kv.Value) : 0;
                     })
                     .Where(v => v != 0);

                if (marks.Count() == 0 || marks.Average() == 0)
                    return 1;

                var av = marks.Average();

                if (av > 0)
                    return Convert.ToInt32(Math.Ceiling(av));

                else
                    return Convert.ToInt32(Math.Floor(av));
            }
            catch
            {
                throw;
            }
        }
    }
}
