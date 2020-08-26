using NewsGatheringService.Domain.Abstract;

namespace NewsGatheringService.Domain.Onliner
{
    public class OnlinerSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://www.onliner.by/";
    }
}
