using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace SpecFlowProject.TestSupport;

public class GmailClient
{
    private readonly UserCredential _credentials;

    public GmailClient()
    {
        _credentials = LoginAsync().Result;
    }

    private async Task<UserCredential> LoginAsync()
    {
        string[] scopes = { GmailService.Scope.GmailReadonly };

        await using var stream = new FileStream("gmail.credentials.json", FileMode.Open, FileAccess.Read);
        var currentDirectory = Directory.GetCurrentDirectory();
        var tokenPath = Path.Combine(currentDirectory[..currentDirectory.LastIndexOf("bin", StringComparison.Ordinal)], "GmailToken");

        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            (await GoogleClientSecrets.FromStreamAsync(stream).ConfigureAwait(false)).Secrets,
            scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(tokenPath, true)).ConfigureAwait(false);

        return credential;
    }

    private async Task<Message> GetLastMessageAsync(string userId)
    {
        try
        {
            var service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credentials,
                ApplicationName = "Gmail Client",
            });

            var listRequest = service.Users.Messages.List(userId);
            listRequest.MaxResults = 1;
            var messages = (await listRequest.ExecuteAsync()).Messages;

            if (messages is { Count: > 0 })
            {
                var message = await service.Users.Messages.Get(userId, messages[0].Id).ExecuteAsync();
                return message;
            }
            else
            {
                Console.WriteLine("No messages found.");
                return null!;
            }
        }
        catch (Google.GoogleApiException e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            return null!;
        }
    }

    public async Task CheckThatEmailWasReceivedAsync(string emailFrom, string emailSubject, int timeout)
    {
        DateTime startTime = DateTime.Now;
        string subject = null!;
        string sender = null!;

        while ((subject != emailSubject || sender != $"<{emailFrom}>") && (DateTime.Now - startTime).TotalSeconds < timeout)
        {
            var message = await this.GetLastMessageAsync("me");

            if (message.Payload is { Headers: not null })
            {
                var headers = message.Payload.Headers;
                foreach (var header in headers)
                {
                    if (header.Name == "Subject")
                    {
                        subject = header.Value;
                    }
                    else if (header.Name == "From")
                    {
                        sender = header.Value;
                    }
                }
            }

            await Task.Delay(1000);
        }

        if (subject == emailSubject && sender == $"<{emailFrom}>")
        {
            return; // Email received successfully.
        }

        throw new TimeoutException($"Email from <{emailFrom}> with subject '{emailSubject}' not received within the timeout.");
    }
}