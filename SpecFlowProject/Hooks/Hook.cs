using Microsoft.Playwright;
using SpecFlowProject.PageObjects;
using SpecFlowProject.TestSupport;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecFlowProject.Hooks
{
    [Binding]
    public class SendEmailHooks
    {
        private readonly string _traceName;
        private readonly ScenarioContext _scenarioContext;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        
        public SendEmailHooks(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _specFlowOutputHelper = specFlowOutputHelper;
            _scenarioContext = scenarioContext;
            _traceName = scenarioContext.ScenarioInfo.Title.Replace(" ", "_");
        }
        
        [BeforeScenario]
        public async Task StartTracingAsync(BasePage basePage)
        {
            var tracing = await basePage.TaskTracing;
            await tracing.StartAsync(new TracingStartOptions
            {
                Name = _traceName,
                Screenshots = true,
                Snapshots = true
            });
        }

        [AfterScenario]
        public async Task StopTracingAsync(BasePage basePage)
        {
            var tracing = await basePage.TaskTracing;
            await tracing.StopAsync(new TracingStopOptions()
            {
                Path = $"Traces/{_traceName}.zip"
            });
        }
        
        [AfterScenario]
        public async Task CreateScreenshotAsync(BasePage basePage)
        {
            if (_scenarioContext.ScenarioExecutionStatus != ScenarioExecutionStatus.OK)
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var page = await basePage.TaskPage;
                var file = await Helpers.TakeScreenshot(page);
                var fullPath = Path.Combine(currentDirectory, file);
                _specFlowOutputHelper.AddAttachment(fullPath);
            }
        }
    }
}