using AngleSharp.Html.Parser;
using NewsGatheringService.Domain.Abstract;
using System.Threading.Tasks;

namespace NewsGatheringService.Domain.Onliner
{
    public class ParserWorker<T> where T : class
    {
        HtmlLoader loader;
        public ParserWorker(IParser<T> parser, string url)
        {
            Parser = parser;
            loader = new HtmlLoader(url);
        }

        #region Properties
        public IParser<T> Parser { get; set; }
        #endregion

        public async Task<T> WorkerAsync()
        {
            var source = await loader.GetSource();
            var domParser = new HtmlParser();

            var document = await domParser.ParseDocumentAsync(source);

            var result = Parser.Parse(document);
            return result;
        }
    }
}
