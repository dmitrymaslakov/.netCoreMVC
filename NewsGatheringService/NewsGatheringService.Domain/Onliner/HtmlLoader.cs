using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsGatheringService.Domain.Onliner
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string _url;

        public HtmlLoader(string url)
        {
            client = new HttpClient();
            _url = url;
        }

        public async Task<string> GetSource()
        {
            var response = await client.GetAsync(_url);
            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}