using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using SpecFlowProject.TestSupport;

namespace SpecFlowProject.PageObjects;

public class BasePage : PageTest
{
    protected new IPage Page { get; private set; }
    public Task<ITracing> TaskTracing { get; private set; }
    public Task<IPage> TaskPage { get; private set; }

    protected BasePage(Driver driver)
    {
        TaskPage = driver.Page;
        Page = driver.Page.Result;
        TaskTracing = driver.Tracing;
    }
}