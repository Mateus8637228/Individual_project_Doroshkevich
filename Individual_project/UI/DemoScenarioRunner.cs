using System;
using System.IO;
using Individual_project.Models;
using Individual_project.Services;

namespace Individual_project.UI
{
  public class DemoScenarioRunner
  {
    private InvoiceGenerator invoiceGenerator;
    private InvoiceTextExporter invoiceExporter;
    private ConsolePrinter consolePrinter;
    private string outputFolderPath;

    public DemoScenarioRunner(
      InvoiceGenerator invoiceGenerator,
      InvoiceTextExporter invoiceExporter,
      ConsolePrinter consolePrinter,
      string outputFolderPath)
    {
      this.invoiceGenerator = invoiceGenerator;
      this.invoiceExporter = invoiceExporter;
      this.consolePrinter = consolePrinter;
      this.outputFolderPath = outputFolderPath;
    }

    public void RunAll()
    {
      RunScenario1_ListTemplates();
      RunScenario2_StandardInvoice();
      RunScenario3_NoVatInvoice();
      RunScenario4_CloneAndModifyTemplate();
      RunScenario5_CloneAndAddItem();

      System.Console.WriteLine();
      System.Console.WriteLine("All demo scenarios completed.");
      System.Console.WriteLine("Files saved to: " + outputFolderPath);
    }

    private void RunScenario1_ListTemplates()
    {
      string scenarioTitle = "=== SCENARIO 1: TEMPLATE REGISTRY ===";
      System.Console.WriteLine(scenarioTitle);

      TemplateRegistry templateRegistry = invoiceGenerator.GetTemplateRegistry();
      consolePrinter.PrintRegistryKeys(templateRegistry);
      System.Console.WriteLine();
    }

    private void RunScenario2_StandardInvoice()
    {
      string scenarioTitle = "=== SCENARIO 2: INVOICE FROM STANDARD TEMPLATE ===";
      System.Console.WriteLine(scenarioTitle);

      string templateKey = "standard";
      string invoiceNumber = "INV-001";
      int issueYear = 2026;
      int issueMonth = 5;
      int issueDay = 24;
      DateTime issueDate = new DateTime(issueYear, issueMonth, issueDay);

      string clientName = "Doroshkevich ME";
      string clientAddress = "Kemerovo, Sovetskiy Ave., 10";

      Invoice invoice = invoiceGenerator.GenerateInvoice(
        templateKey,
        invoiceNumber,
        issueDate,
        clientName,
        clientAddress);

      SaveAndPrint(invoice, "invoice-001.txt");
    }

    private void RunScenario3_NoVatInvoice()
    {
      string scenarioTitle = "=== SCENARIO 3: INVOICE WITHOUT VAT ===";
      System.Console.WriteLine(scenarioTitle);

      string templateKey = "no-vat";
      string invoiceNumber = "INV-002";
      int issueYear = 2026;
      int issueMonth = 5;
      int issueDay = 25;
      DateTime issueDate = new DateTime(issueYear, issueMonth, issueDay);

      string clientName = "IP Petrov";
      string clientAddress = "Kemerovo, Trade St., 5";

      Invoice invoice = invoiceGenerator.GenerateInvoice(
        templateKey,
        invoiceNumber,
        issueDate,
        clientName,
        clientAddress);

      SaveAndPrint(invoice, "invoice-002.txt");
    }

    private void RunScenario4_CloneAndModifyTemplate()
    {
      string scenarioTitle = "=== SCENARIO 4: CLONE TEMPLATE AND CHANGE VAT ===";
      System.Console.WriteLine(scenarioTitle);

      string templateKey = "standard";
      InvoiceTemplate clonedTemplate = invoiceGenerator.GetClonedTemplate(templateKey);

      string modifiedTemplateName = "Standard services (custom VAT)";
      clonedTemplate.TemplateName = modifiedTemplateName;

      decimal customVatRate = 10.0m;
      clonedTemplate.DefaultVatRatePercent = customVatRate;

      consolePrinter.PrintTemplateInfo(clonedTemplate);
      System.Console.WriteLine();

      string invoiceNumber = "INV-003";
      int issueYear = 2026;
      int issueMonth = 5;
      int issueDay = 26;
      DateTime issueDate = new DateTime(issueYear, issueMonth, issueDay);

      string clientName = "IP Sidorov";
      string clientAddress = "Kemerovo, Business St., 3";

      Invoice invoice = clonedTemplate.CreateInvoice(
        invoiceNumber,
        issueDate,
        clientName,
        clientAddress);

      SaveAndPrint(invoice, "invoice-003.txt");
    }

    private void RunScenario5_CloneAndAddItem()
    {
      string scenarioTitle = "=== SCENARIO 5: CLONE TEMPLATE AND ADD ITEM ===";
      System.Console.WriteLine(scenarioTitle);

      string templateKey = "standard";
      InvoiceTemplate clonedTemplate = invoiceGenerator.GetClonedTemplate(templateKey);

      string extraItemName = "Technical support";
      int extraItemQuantity = 3;
      decimal extraItemUnitPrice = 50.0m;
      InvoiceItem extraItem = new InvoiceItem(extraItemName, extraItemQuantity, extraItemUnitPrice);
      clonedTemplate.AddDefaultItem(extraItem);

      consolePrinter.PrintTemplateInfo(clonedTemplate);
      System.Console.WriteLine();

      string invoiceNumber = "INV-004";
      int issueYear = 2026;
      int issueMonth = 5;
      int issueDay = 27;
      DateTime issueDate = new DateTime(issueYear, issueMonth, issueDay);

      string clientName = "IP Kozlov";
      string clientAddress = "Kemerovo, Office St., 7";

      Invoice invoice = clonedTemplate.CreateInvoice(
        invoiceNumber,
        issueDate,
        clientName,
        clientAddress);

      SaveAndPrint(invoice, "invoice-004.txt");
    }

    private void SaveAndPrint(Invoice invoice, string fileName)
    {
      consolePrinter.PrintInvoice(invoice);

      string filePath = Path.Combine(outputFolderPath, fileName);
      invoiceExporter.SaveToFile(invoice, filePath);

      string savedMessage = "Saved: " + filePath;
      System.Console.WriteLine(savedMessage);
      System.Console.WriteLine();
    }
  }
}
