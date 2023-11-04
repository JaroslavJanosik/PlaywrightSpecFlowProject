using Microsoft.Playwright;
using SpecFlowProject.TestSupport;

namespace SpecFlowProject.PageObjects
{
    public class LoginPage : BasePage
    {
        private ILocator LogInSection => Page.Locator("#login");
        private ILocator UserName => Page.Locator("#login-username");
        private ILocator Password => Page.Locator("#login-password");
        private ILocator SignInButton => Page.Locator("(//button[@data-locale='login.submit'])[1]");
        
        public LoginPage(Driver driver) : base(driver) {}
 
        public async Task Open(string url) => await Page.GotoAsync(url);
        
        public async Task CheckThatLoginPageIsLoaded() => await Expect(LogInSection).ToBeVisibleAsync();

        public async Task LoginToEmail(string userName, string password)
        { 
            await UserName.FillAsync(userName);
            await SignInButton.ClickAsync();
            await Password.FillAsync(password);
            await SignInButton.ClickAsync();
        }
    }
}