using NewsGatheringService.DAL.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Helpers
{
    public class NewsEvaluation
    {
        private static readonly Dictionary<string, string> _affin;

        static NewsEvaluation()
        {
            using (var r = new StreamReader(@"F:\Разное\Учёба\Программирование Си шарп\It-academy\Projects\NewsGatheringService\NewsCollector.BLL\AFINN-ru.json"))
            {
                string json = r.ReadToEnd();
                _affin = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                r.Close();
            }
        }
        /// <summary>
        /// Evaluates news based on affin
        /// </summary>
        /// <param name="news">News to be evaluated</param>
        /// <returns>value in the range of -5 to 5</returns>
        public static async Task<int> EvaluateNewsAsync(News news)
        {
            /*var headLemma = await NewsTextLemmatization.RunAsync(news.NewsStructure.Headline);
            var leadLemma = await NewsTextLemmatization.RunAsync(news.NewsStructure.Lead);*/
            var bodyLemma = await NewsTextLemmatization.RunAsync(news.NewsStructure.Body);
            /*var headValue = evaluateLemma(headLemma);
            var leadValue = evaluateLemma(leadLemma);*/
            var bodyValue = evaluateLemma(bodyLemma);

            return bodyValue;
        }

        private static int evaluateLemma(string lemma)
        {
            if (string.IsNullOrEmpty(lemma)) return 0;

            var q =
                Convert.ToInt32(
                Math.Ceiling(
                _affin
                .Where(kv => !string.IsNullOrEmpty(kv.Key))
                .Select(kv =>
                {
                    var b = Regex.IsMatch(lemma, "\\b" + kv.Key + "\\b");
                    return b ? int.Parse(kv.Value) : 0;
                })
                .Where(v => v != 0)
                .Average()
                )
                )
                ;
            return q;
        }
    }
}
