using Microsoft.Playwright;
using NUnit.Framework;
using SpecFlowProject.TestSupport;

namespace SpecFlowProject.PageObjects
{
    public class HomePage : BasePage
    {
        private ILocator SeznamEmailTitle => Page.Locator("a[title='Seznam Email']");
        private ILocator ComposeEmailButton => Page.Locator("a[data-command='compose:new']");
        private ILocator ModalWindow => Page.Locator("section[class='compose-full animate-up']");
        private ILocator SendEmailButton => Page.Locator("button[data-command='compose:send']:not([class='mobile'])");
        private ILocator RecipientField => Page.Locator("input[placeholder='Komu…']");
        private ILocator SubjectField => Page.Locator("input[placeholder='Předmět…']");
        private ILocator EmailBodyField => Page.Locator("div[placeholder='Text e-mailu…']");
        private ILocator FileUploadField => Page.Locator("button[title='Přidat přílohu']");
        private ILocator Attachment => Page.Locator("li[class='attachment']");
        private ILocator SentEmailsList => Page.Locator("#list");
        private ILocator SentEmailsNav => Page.Locator("a[title='Odeslané']");
        private ILocator LastSentEmailName => Page.Locator("(//a[@class='name'])[1]");
        private ILocator LastSentEmailSubject => Page.Locator("(//a[@class='subject'])[1]");
        private ILocator Notification => Page.Locator("div.notification");
        private ILocator LogInSection => Page.Locator("#login");
        private ILocator UsersButton => Page.Locator("#badge");
        private ILocator LogOutButton => Page.Locator("[data-dot='logout']");

        public HomePage(Driver driver) : base(driver) {}

        public async Task CheckThatHomePageIsLoaded() => await Expect(SeznamEmailTitle).ToBeVisibleAsync();

        public async Task ClickOnComposeEmailButton() => await ComposeEmailButton.ClickAsync();

        public async Task CheckThatModalWindowIsOpen() => await Expect(ModalWindow).ToBeVisibleAsync();

        public async Task FillInEmailFields(string recipient, string subject, string emailBody)
        {
            await RecipientField.FillAsync(recipient);
            await SubjectField.FillAsync(subject);
            await EmailBodyField.FillAsync(emailBody);
        }

        public async Task AddAttachment(string fileUploadPath)
        {
            await FileUploadField.ClickAsync();
            var fileChooser = await Page.RunAndWaitForFileChooserAsync(async () =>
            {
                await FileUploadField.ClickAsync();
            });
            await fileChooser.SetFilesAsync(fileUploadPath);
            await Expect(Attachment).ToBeVisibleAsync();
        }

        public async Task ClickOnSendEmailButton() => await SendEmailButton.ClickAsync();

        public async Task CheckThatEmailNotificationIsVisible() => await Expect(Notification).ToBeVisibleAsync();
  
        public async Task CheckThatModalWindowIsClosed() => await Expect(ModalWindow).ToBeHiddenAsync();
        
        public async Task ClickOnSentButton() => await SentEmailsNav.ClickAsync();

        public async Task CheckThatSentEmailsListIsVisible() => await Expect(SentEmailsList).ToBeVisibleAsync();

        public void AssertThatEmailIsPresentInList(string recipient, string subject)
        {
            Assert.That(LastSentEmailName.InnerTextAsync().Result, Is.EqualTo(recipient));
            Assert.That(LastSentEmailSubject.InnerTextAsync().Result, Is.EqualTo(subject));
        }
        
        public async Task SendEmail(string recipient, string subject, string emailBody, string fileUploadPath)
        {
            await ClickOnComposeEmailButton();
            await CheckThatModalWindowIsOpen();
            await FillInEmailFields(recipient, subject, emailBody);
            await AddAttachment(fileUploadPath);
            await ClickOnSendEmailButton();
            await CheckThatEmailNotificationIsVisible();
            await CheckThatModalWindowIsClosed();
        }

        public async Task CheckThatEmailWasSent(string recipient, string subject)
        {
            await ClickOnSentButton();
            await CheckThatSentEmailsListIsVisible();
            AssertThatEmailIsPresentInList(recipient,subject);
        }

        public async Task LogOut()
        {
            await UsersButton.ClickAsync();
            await LogOutButton.ClickAsync();
            await Expect(LogInSection).ToBeVisibleAsync();
        }
    }
}
