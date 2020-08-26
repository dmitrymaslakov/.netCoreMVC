using AngleSharp.Html.Dom;

namespace NewsGatheringService.Domain.Abstract
{
    public interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
} 