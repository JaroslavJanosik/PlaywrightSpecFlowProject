using Microsoft.Playwright;

namespace SpecFlowProject.TestSupport
{
    public class DriverInit
    {
        private const float DefaultTimeout = 30f;

        public async Task<IBrowser> GetChromeDriverAsync(string[]? args = null, float? timeout = DefaultTimeout, bool? headless = true, float? slowmo = null, string? traceDir = null)
        {
            var options = GetOptions(args, timeout, headless, slowmo, traceDir);
            options.Channel = "chrome";

            return await GetBrowserAsync(BrowserType.Chromium, options);
        }

        public async Task<IBrowser> GetFirefoxDriverAsync(string[]? args = null, float? timeout = DefaultTimeout, bool? headless = true, float? slowmo = null, string? traceDir = null)
        {
            var options = GetOptions(args, timeout, headless, slowmo, traceDir);

            return await GetBrowserAsync(BrowserType.Firefox, options);
        }

        public async Task<IBrowser> GetEdgeDriverAsync(string[]? args = null, float? timeout = DefaultTimeout, bool? headless = true, float? slowmo = null, string? traceDir = null)
        {
            var options = GetOptions(args, timeout, headless, slowmo, traceDir);
            options.Channel = "msedge";

            return await GetBrowserAsync(BrowserType.Chromium, options);
        }

        public async Task<IBrowser> GetChromiumDriverAsync(string[]? args, float? timeout = DefaultTimeout, bool? headless = true, float? slowmo = null, string? traceDir = null)
        {
            var options = GetOptions(args, timeout, headless, slowmo, traceDir);

            return await GetBrowserAsync(BrowserType.Chromium, options);
        }

        public async Task<IBrowser> GetWebKitDriverAsync(string[]? args, float? timeout = DefaultTimeout, bool? headless = true, float? slowmo = null, string? traceDir = null)
        {
            var options = GetOptions(args, timeout, headless, slowmo, traceDir);

            return await GetBrowserAsync(BrowserType.Webkit, options);
        }

        private static BrowserTypeLaunchOptions GetOptions(string[]? args, float? timeout = DefaultTimeout, bool? headless = true, float? slowmo = null, string? traceDir = null)
            => new()
            { Args = args, Timeout = ToMilliseconds(timeout), Headless = headless, SlowMo = slowmo, TracesDir = traceDir};

        private static async Task<IBrowser> GetBrowserAsync(string browserType, BrowserTypeLaunchOptions options)
        {
            var playwright = await Playwright.CreateAsync();

            return await playwright[browserType].LaunchAsync(options);
        }

        private static float? ToMilliseconds(float? seconds)
        {
            return seconds * 1000;
        }
    }
}