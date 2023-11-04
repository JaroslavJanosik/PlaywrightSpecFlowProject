using System.Reflection;
using SpecFlowProject.TestSupport;

namespace SpecFlowProject;

public class LivingDocGenerator
{
    private static void Main()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var projectName = Assembly.GetExecutingAssembly().GetName().Name;
        var testAssemblyPath = Path.Combine(currentDirectory, projectName + ".dll");
        var testExecutionJsonPath = Path.Combine(currentDirectory, "TestExecution.json");
        var testReportPath = Path.Combine(currentDirectory[..currentDirectory.LastIndexOf("bin", StringComparison.Ordinal)], "HtmlReport");
        
        var command = "livingdoc";
        var arguments = $"test-assembly {testAssemblyPath} -t {testExecutionJsonPath} -o {testReportPath}";
        
        var exitCode = Helpers.RunCommand(command, arguments);
        
        Console.WriteLine($"Exit Code: {exitCode}");
    }
}