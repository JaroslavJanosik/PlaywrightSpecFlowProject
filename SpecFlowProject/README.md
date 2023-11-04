Simple Automation Framework based on C#, NUnit, SpecFlow and Playwright.

### How to use this framework?
- clone the repository to your workspace
- open the project (SendEmailProject.sln) in a compatible IDE, such as Microsoft Visual Studio or Rider
- download the project dependencies
- set a Browser in the pw.settings.json file
- build the solution
- run the test using Test Explorer

### Test case
```
Feature: Seznam Email

    Scenario: Sending an Email with an Attachment

        Given the user is on the application's login page
        When the user logs in with valid credentials
        Then the home page should load successfully
        When the user clicks on the Compose e-mail button in the navigation panel
        Then a modal window should open
        When the user fills in the recipient, subject, and email body
        And attaches a file
        And clicks on the Send e-mail button
        Then the email should be sent successfully
        And the modal window should close
        And a notification message should be displayed
        When the user clicks on the Sent button in the navigation panel
        Then a list of sent emails should appear in the content section
        And the sent email should be the most recent item in the list
        And the recipient should receive the email
        When the user logs out from the application
        Then they should be returned to the application's login page
  ```