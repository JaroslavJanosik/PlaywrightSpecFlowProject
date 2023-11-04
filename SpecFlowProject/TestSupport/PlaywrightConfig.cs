using Microsoft.Extensions.Configuration;

namespace SpecFlowProject.TestSupport
{
    public class PlaywrightConfig
    {
        public Browser Browser { get; set; }
        public string[]? Arguments { get; set; } 
        public float? DefaultTimeout { get; set; }
        public bool? Headless { get; set; }
        public float? SlowMo { get; set; }
        public string? TraceDir { get; set; }
        
        public PlaywrightConfig AddConfig(string path)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(path).Build();

            return config.Get<PlaywrightConfig>()!;
        }
    }
}