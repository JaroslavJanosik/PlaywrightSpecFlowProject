using Microsoft.Playwright;

namespace SpecFlowProject.TestSupport;

public class Driver : IAsyncDisposable
{
    private readonly DriverInit _driverInit;
    private readonly PlaywrightConfig _playwrightConfig;
    private readonly Task<IBrowser> _browserInstance;
    public Task<IPage> Page { get; private set; }
    public Task<ITracing> Tracing { get; private set; }

    public Driver(DriverInit driverInit, PlaywrightConfig playwrightConfig)
    {
        _driverInit = driverInit;
        _playwrightConfig = playwrightConfig.AddConfig("pw.settings.json");
        _browserInstance = BrowserInit(_playwrightConfig);
        var browserContext = CreateBrowserContextAsync(_browserInstance);
        Tracing = CreateTracingAsync(browserContext);
        Page = CreatePageAsync(browserContext);
    }

    public async ValueTask DisposeAsync()
    {
        var browser = await _browserInstance; 
        await browser.CloseAsync();
    }
    
    private async Task<IBrowser> BrowserInit(PlaywrightConfig config)
    {
        return config.Browser switch
        {
            Browser.Chrome => await _driverInit.GetChromeDriverAsync(_playwrightConfig.Arguments, _playwrightConfig.DefaultTimeout, _playwrightConfig.Headless, _playwrightConfig.SlowMo, _playwrightConfig.TraceDir),
            Browser.Firefox => await _driverInit.GetFirefoxDriverAsync(_playwrightConfig.Arguments, _playwrightConfig.DefaultTimeout, _playwrightConfig.Headless, _playwrightConfig.SlowMo, _playwrightConfig.TraceDir),
            Browser.Edge => await _driverInit.GetEdgeDriverAsync(_playwrightConfig.Arguments, _playwrightConfig.DefaultTimeout, _playwrightConfig.Headless, _playwrightConfig.SlowMo, _playwrightConfig.TraceDir),
            Browser.Chromium => await _driverInit.GetChromiumDriverAsync(_playwrightConfig.Arguments, _playwrightConfig.DefaultTimeout, _playwrightConfig.Headless, _playwrightConfig.SlowMo, _playwrightConfig.TraceDir),
            Browser.Webkit => await _driverInit.GetWebKitDriverAsync(_playwrightConfig.Arguments, _playwrightConfig.DefaultTimeout, _playwrightConfig.Headless, _playwrightConfig.SlowMo, _playwrightConfig.TraceDir),
            _ => throw new NotImplementedException($"Support for browser {_playwrightConfig.Browser} is not implemented yet"),
        };
    }
    
    private static async Task<IBrowserContext> CreateBrowserContextAsync(Task<IBrowser> browser)
    {
        var actualBrowser = await browser;
        return await actualBrowser.NewContextAsync();
    }

    private static async Task<ITracing> CreateTracingAsync(Task<IBrowserContext> browserContext)
    {
        return await browserContext.ContinueWith(t => t.Result.Tracing);
    }

    private static async Task<IPage> CreatePageAsync(Task<IBrowserContext> browserContext)
    {
        var context = await browserContext;
        return await context.NewPageAsync();
    }
}