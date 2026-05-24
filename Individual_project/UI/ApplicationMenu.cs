using System;
using System.IO;
using Individual_project.Models;
using Individual_project.Services;

namespace Individual_project.UI
{
  public class ApplicationMenu
  {
    private InvoiceGenerator invoiceGenerator;
    private InvoiceTextExporter invoiceExporter;
    private ConsoleInputReader inputReader;
    private ConsolePrinter consolePrinter;
    private DemoScenarioRunner demoRunner;
    private string outputFolderPath;

    private int menuChoiceListTemplates = 1;
    private int menuChoiceCreateInvoice = 2;
    private int menuChoiceCloneTemplate = 3;
    private int menuChoiceRunDemo = 4;
    private int menuChoiceExit = 0;
    private int menuChoiceMax = 4;

    public ApplicationMenu(
      InvoiceGenerator invoiceGenerator,
      InvoiceTextExporter invoiceExporter,
      ConsoleInputReader inputReader,
      ConsolePrinter consolePrinter,
      DemoScenarioRunner demoRunner,
      string outputFolderPath)
    {
      this.invoiceGenerator = invoiceGenerator;
      this.invoiceExporter = invoiceExporter;
      this.inputReader = inputReader;
      this.consolePrinter = consolePrinter;
      this.demoRunner = demoRunner;
      this.outputFolderPath = outputFolderPath;
    }

    public void Run()
    {
      bool isRunning = true;

      while (isRunning) {
        PrintMainMenu();
        int choice = inputReader.ReadMenuChoice(menuChoiceExit, menuChoiceMax);

        if (choice == menuChoiceListTemplates) {
          HandleListTemplates();
        } else if (choice == menuChoiceCreateInvoice) {
          HandleCreateInvoice();
        } else if (choice == menuChoiceCloneTemplate) {
          HandleCloneTemplate();
        } else if (choice == menuChoiceRunDemo) {
          demoRunner.RunAll();
        } else if (choice == menuChoiceExit) {
          isRunning = false;
        }

        System.Console.WriteLine();
      }

      System.Console.WriteLine("Goodbye.");
    }

    private void PrintMainMenu()
    {
      System.Console.WriteLine("=== INVOICE GENERATOR ===");
      System.Console.WriteLine(menuChoiceListTemplates + ". List templates");
      System.Console.WriteLine(menuChoiceCreateInvoice + ". Create invoice from template");
      System.Console.WriteLine(menuChoiceCloneTemplate + ". Clone template and create invoice");
      System.Console.WriteLine(menuChoiceRunDemo + ". Run demo scenarios");
      System.Console.WriteLine(menuChoiceExit + ". Exit");
      System.Console.WriteLine();
    }

    private void HandleListTemplates()
    {
      System.Console.WriteLine("=== TEMPLATES ===");
      TemplateRegistry templateRegistry = invoiceGenerator.GetTemplateRegistry();
      consolePrinter.PrintRegistryKeys(templateRegistry);
    }

    private void HandleCreateInvoice()
    {
      string templateKey = ReadTemplateKey();
      if (templateKey == null) {
        return;
      }

      string invoiceNumber = inputReader.ReadRequiredLine("Invoice number: ");
      DateTime issueDate = inputReader.ReadDate("Issue date");

      string defaultClientName = "Doroshkevich ME";
      string defaultClientAddress = "Kemerovo, Sovetskiy Ave., 10";
      string clientName = inputReader.ReadLineWithDefault("Client name", defaultClientName);
      string clientAddress = inputReader.ReadLineWithDefault("Client address", defaultClientAddress);

      Invoice invoice = invoiceGenerator.GenerateInvoice(
        templateKey,
        invoiceNumber,
        issueDate,
        clientName,
        clientAddress);

      SaveInvoice(invoice, invoiceNumber);
    }

    private void HandleCloneTemplate()
    {
      string templateKey = ReadTemplateKey();
      if (templateKey == null) {
        return;
      }

      InvoiceTemplate clonedTemplate = invoiceGenerator.GetClonedTemplate(templateKey);

      System.Console.WriteLine("Cloned template. You can change VAT rate.");
      string changeVatPrompt = "Change VAT rate? (1 = yes, 0 = no): ";
      int minChoice = 0;
      int maxChoice = 1;
      int changeVatChoice = inputReader.ReadIntInRange(changeVatPrompt, minChoice, maxChoice);

      int yesChoice = 1;
      if (changeVatChoice == yesChoice) {
        string vatPrompt = "New VAT rate (%): ";
        decimal newVatRate = inputReader.ReadDecimal(vatPrompt);
        clonedTemplate.DefaultVatRatePercent = newVatRate;
      }

      string addItemPrompt = "Add extra item to cloned template? (1 = yes, 0 = no): ";
      int addItemChoice = inputReader.ReadIntInRange(addItemPrompt, minChoice, maxChoice);

      if (addItemChoice == yesChoice) {
        string itemName = inputReader.ReadRequiredLine("Item name: ");
        string quantityPrompt = "Quantity: ";
        int minQuantity = 1;
        int maxQuantity = 100000;
        int itemQuantity = inputReader.ReadIntInRange(quantityPrompt, minQuantity, maxQuantity);
        string pricePrompt = "Unit price: ";
        decimal itemUnitPrice = inputReader.ReadDecimal(pricePrompt);

        InvoiceItem extraItem = new InvoiceItem(itemName, itemQuantity, itemUnitPrice);
        clonedTemplate.AddDefaultItem(extraItem);
      }

      consolePrinter.PrintTemplateInfo(clonedTemplate);
      System.Console.WriteLine();

      string invoiceNumber = inputReader.ReadRequiredLine("Invoice number: ");
      DateTime issueDate = inputReader.ReadDate("Issue date");
      string clientName = inputReader.ReadRequiredLine("Client name: ");
      string clientAddress = inputReader.ReadRequiredLine("Client address: ");

      Invoice invoice = clonedTemplate.CreateInvoice(
        invoiceNumber,
        issueDate,
        clientName,
        clientAddress);

      SaveInvoice(invoice, invoiceNumber);
    }

    private string ReadTemplateKey()
    {
      System.Console.WriteLine("Available templates:");
      TemplateRegistry templateRegistry = invoiceGenerator.GetTemplateRegistry();
      consolePrinter.PrintRegistryKeys(templateRegistry);
      System.Console.WriteLine();

      string templateKey = inputReader.ReadRequiredLine("Template key: ");

      if (!templateRegistry.Contains(templateKey)) {
        System.Console.WriteLine("Template not found: " + templateKey);
        templateKey = null;
      }

      return templateKey;
    }

    private void SaveInvoice(Invoice invoice, string invoiceNumber)
    {
      consolePrinter.PrintInvoice(invoice);

      string safeFileName = invoiceNumber.Replace(" ", "_");
      string fileName = safeFileName + ".txt";
      string filePath = Path.Combine(outputFolderPath, fileName);
      invoiceExporter.SaveToFile(invoice, filePath);

      string savedMessage = "Saved: " + filePath;
      System.Console.WriteLine(savedMessage);
    }
  }
}
