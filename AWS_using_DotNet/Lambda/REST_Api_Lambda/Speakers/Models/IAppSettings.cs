namespace Speakers.Models
{
    public interface IAppSettings
    {
        string ApplicationInsightConnectionString { get; set; }
    }
    public class AppSettings : IAppSettings
    {
        public string ApplicationInsightConnectionString { get; set; } = string.Empty;
    }
}
