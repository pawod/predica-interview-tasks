namespace WarsawBrowser.Models
{
    public class AppSettings
    {
        public readonly long MinLinuxUtcTimestamp = 1393628401000;
        public string WarsawApiUrl { get; set; }
        public string WarsawApiView { get; set; }
        public string WarsawApiKey { get; set; }

        public AppSettings() { }
    }
}
