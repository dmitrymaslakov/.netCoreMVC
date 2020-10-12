namespace NewsGatheringService.DAL.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string AFINN_ruFilePath { get; set; }
        public string LemmaKey { get; set; }
        public string[] RssFeeds { get; set; }
        public string AppsettingsFilePath { get; set; }
    }
}
