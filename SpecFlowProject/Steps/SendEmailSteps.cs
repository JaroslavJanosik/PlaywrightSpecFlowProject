using SpecFlowProject.PageObjects;
using SpecFlowProject.TestSupport;

namespace SpecFlowProject.Steps;

[Binding]
public class SendEmailSteps
{
    private readonly TestConfig _testConfig;
    private readonly HomePage _homePage;
    private readonly LoginPage _loginPage;
    private readonly GmailClient _gmailClient;
    
    private readonly string _emailSubject = $"Test Email {DateTime.UtcNow:yyyymmddhhmmss}";
    private readonly string _fileUploadPath = Path.GetFullPath("TestData/attachment.txt");
    private const string EmailBody = "Hi,\n\nthis is a test email.\n\nKind Regards\n\nJaroslav";

    public SendEmailSteps(Driver driver, TestConfig testConfig)
    {
        _testConfig = testConfig.AddConfig("test.settings.json");
        _gmailClient = new GmailClient();
        _loginPage = new LoginPage(driver);
        _homePage = new HomePage(driver);
    }
    
    [Given(@"the user is on the application's login page")]
    public async Task GivenTheUserIsOnTheApplicationsLoginPage()
    { 
        await _loginPage.Open(_testConfig.BaseUrl!);
        await _loginPage.CheckThatLoginPageIsLoaded();
    }
    
    [When(@"the user logs in with valid credentials")]
    public async Task WhenTheUserLogsInWithValidCredentials()
    {
        await _loginPage.LoginToEmail(_testConfig.Username!, _testConfig.Password!);
    }

    [Then(@"the home page should load successfully")]
    public async Task ThenTheHomePageShouldLoadSuccessfully()
    {
        await _homePage.CheckThatHomePageIsLoaded();
    }

    [When(@"the user clicks on the Compose e-mail button in the navigation panel")]
    public async Task WhenTheUserClicksOnTheComposeButtonInTheNavigationPanel()
    {
        await _homePage.ClickOnComposeEmailButton();
    }

    [Then(@"a modal window should open")]
    public async Task ThenAModalWindowShouldOpen()
    {
        await _homePage.CheckThatModalWindowIsOpen();
    }

    [When(@"the user fills in the recipient, subject, and email body")]
    public async Task WhenTheUserFillsInTheRecipientSubjectAndEmailBody()
    {
        await _homePage.FillInEmailFields(_testConfig.RecipientEmail!, _emailSubject, EmailBody);
    }

    [When(@"attaches a file")]
    public async Task WhenAttachesAFile()
    {
        await _homePage.AddAttachment(_fileUploadPath);
    }

    [When(@"clicks on the Send e-mail button")]
    public async Task WhenClicksTheButton()
    {
        await _homePage.ClickOnSendEmailButton();
    }

    [Then(@"the email should be sent successfully")]
    public async Task ThenTheEmailShouldBeSentSuccessfully()
    {
        await _homePage.CheckThatEmailWasSent(_testConfig.RecipientEmail!, _emailSubject);
    }
    
    [Then(@"the modal window should close")]
    public async Task ThenTheModalWindowShouldClose()
    {
        await _homePage.CheckThatModalWindowIsClosed();
    }

    [Then(@"a notification message should be displayed")]
    public async Task ThenANotificationMessageShouldBeDisplayed()
    {
        await _homePage.CheckThatEmailNotificationIsVisible();
    }

    [When(@"the user clicks on the Sent button in the navigation panel")]
    public async Task WhenTheUserClicksOnTheSentButtonInTheNavigationPanel()
    {
        await _homePage.ClickOnSentButton();
    }

    [Then(@"a list of sent emails should appear in the content section")]
    public async Task ThenAListOfSentEmailsShouldAppearInTheContentSection()
    {
        await _homePage.CheckThatSentEmailsListIsVisible();
    }

    [Then(@"the sent email should be the most recent item in the list")]
    public void ThenTheSentEmailShouldBeTheMostRecentItemInTheList()
    {
       _homePage.AssertThatEmailIsPresentInList(_testConfig.RecipientEmail!, _emailSubject);
    }
    
    [Then(@"the recipient should receive the email")]
    public async Task ThenTheRecipientShouldReceiveTheEmail()
    {
      await _gmailClient.CheckThatEmailWasReceivedAsync(_testConfig.UserEmail!, _emailSubject, 60);
    }

    [When(@"the user logs out from the application")]
    public async Task WhenTheUserLogsOutFromTheApplication()
    {
        await _homePage.LogOut();
    }

    [Then(@"they should be returned to the application's login page")]
    public async Task ThenTheyShouldBeReturnedToTheApplicationsLoginPage()
    {
        await _loginPage.CheckThatLoginPageIsLoaded();
    }
}