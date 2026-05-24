using System;
using Individual_project.Factories;
using Individual_project.Helpers;
using Individual_project.Services;
using Individual_project.UI;

namespace Individual_project
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      InvoiceGenerator invoiceGenerator = new InvoiceGenerator();
      TemplateFactory.RegisterDefaultTemplates(invoiceGenerator);

      InvoiceTextExporter invoiceExporter = new InvoiceTextExporter();
      ConsoleInputReader inputReader = new ConsoleInputReader();
      ConsolePrinter consolePrinter = new ConsolePrinter(invoiceExporter);
      string outputFolderPath = OutputFolderHelper.PrepareOutputFolder();

      DemoScenarioRunner demoRunner = new DemoScenarioRunner(
        invoiceGenerator,
        invoiceExporter,
        consolePrinter,
        outputFolderPath);

      ApplicationMenu applicationMenu = new ApplicationMenu(
        invoiceGenerator,
        invoiceExporter,
        inputReader,
        consolePrinter,
        demoRunner,
        outputFolderPath);

      applicationMenu.Run();
    }
  }
}
