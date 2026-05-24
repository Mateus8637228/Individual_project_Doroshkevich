using System;
using System.Collections.Generic;
using System.IO;
using Individual_project.Models;
using Individual_project.Services;

namespace Individual_project
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      InvoiceGenerator invoiceGenerator = new InvoiceGenerator();
      InvoiceTextExporter invoiceExporter = new InvoiceTextExporter();
      string outputFolderPath = PrepareOutputFolder();

      RegisterDefaultTemplates(invoiceGenerator);

      RunScenario1_ListTemplates(invoiceGenerator);
      RunScenario2_StandardInvoice(invoiceGenerator, invoiceExporter, outputFolderPath);
      RunScenario3_NoVatInvoice(invoiceGenerator, invoiceExporter, outputFolderPath);
      RunScenario4_CloneAndModifyTemplate(invoiceGenerator, invoiceExporter, outputFolderPath);
      RunScenario5_CloneAndAddItem(invoiceGenerator, invoiceExporter, outputFolderPath);

      Console.WriteLine();
      Console.WriteLine("All scenarios completed. Files saved to: " + outputFolderPath);
      Console.WriteLine("Press Enter to exit...");
      Console.ReadLine();
    }

    static string PrepareOutputFolder()
    {
      string outputFolderName = "output";
      string outputFolderPath = Path.Combine(Directory.GetCurrentDirectory(), outputFolderName);

      if (!Directory.Exists(outputFolderPath)) {
        Directory.CreateDirectory(outputFolderPath);
      }

      return outputFolderPath;
    }

    static void RegisterDefaultTemplates(InvoiceGenerator invoiceGenerator)
    {
      string standardTemplateKey = "standard";
      string noVatTemplateKey = "no-vat";

      InvoiceTemplate standardTemplate = BuildStandardTemplate();
      InvoiceTemplate noVatTemplate = BuildNoVatTemplate();

      invoiceGenerator.RegisterTemplate(standardTemplateKey, standardTemplate);
      invoiceGenerator.RegisterTemplate(noVatTemplateKey, noVatTemplate);
    }

    static void RunScenario1_ListTemplates(InvoiceGenerator invoiceGenerator)
    {
      string scenarioTitle = "=== SCENARIO 1: TEMPLATE REGISTRY ===";
      Console.WriteLine(scenarioTitle);

      TemplateRegistry templateRegistry = invoiceGenerator.GetTemplateRegistry();
      PrintRegistryKeys(templateRegistry);
      Console.WriteLine();
    }

    static void RunScenario2_StandardInvoice(
      InvoiceGenerator invoiceGenerator,
      InvoiceTextExporter invoiceExporter,
      string outputFolderPath)
    {
      string scenarioTitle = "=== SCENARIO 2: INVOICE FROM STANDARD TEMPLATE ===";
      Console.WriteLine(scenarioTitle);

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

      PrintInvoiceToConsole(invoice, invoiceExporter);

      string fileName = "invoice-001.txt";
      string filePath = Path.Combine(outputFolderPath, fileName);
      invoiceExporter.SaveToFile(invoice, filePath);

      string savedMessage = "Saved: " + filePath;
      Console.WriteLine(savedMessage);
      Console.WriteLine();
    }

    static void RunScenario3_NoVatInvoice(
      InvoiceGenerator invoiceGenerator,
      InvoiceTextExporter invoiceExporter,
      string outputFolderPath)
    {
      string scenarioTitle = "=== SCENARIO 3: INVOICE WITHOUT VAT ===";
      Console.WriteLine(scenarioTitle);

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

      PrintInvoiceToConsole(invoice, invoiceExporter);

      string fileName = "invoice-002.txt";
      string filePath = Path.Combine(outputFolderPath, fileName);
      invoiceExporter.SaveToFile(invoice, filePath);

      string savedMessage = "Saved: " + filePath;
      Console.WriteLine(savedMessage);
      Console.WriteLine();
    }

    static void RunScenario4_CloneAndModifyTemplate(
      InvoiceGenerator invoiceGenerator,
      InvoiceTextExporter invoiceExporter,
      string outputFolderPath)
    {
      string scenarioTitle = "=== SCENARIO 4: CLONE TEMPLATE AND CHANGE VAT ===";
      Console.WriteLine(scenarioTitle);

      string templateKey = "standard";
      InvoiceTemplate clonedTemplate = invoiceGenerator.GetClonedTemplate(templateKey);

      string modifiedTemplateName = "Standard services (custom VAT)";
      clonedTemplate.TemplateName = modifiedTemplateName;

      decimal customVatRate = 10.0m;
      clonedTemplate.DefaultVatRatePercent = customVatRate;

      PrintTemplateInfo(clonedTemplate);
      Console.WriteLine();

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

      PrintInvoiceToConsole(invoice, invoiceExporter);

      string fileName = "invoice-003.txt";
      string filePath = Path.Combine(outputFolderPath, fileName);
      invoiceExporter.SaveToFile(invoice, filePath);

      string savedMessage = "Saved: " + filePath;
      Console.WriteLine(savedMessage);
      Console.WriteLine();
    }

    static void RunScenario5_CloneAndAddItem(
      InvoiceGenerator invoiceGenerator,
      InvoiceTextExporter invoiceExporter,
      string outputFolderPath)
    {
      string scenarioTitle = "=== SCENARIO 5: CLONE TEMPLATE AND ADD ITEM ===";
      Console.WriteLine(scenarioTitle);

      string templateKey = "standard";
      InvoiceTemplate clonedTemplate = invoiceGenerator.GetClonedTemplate(templateKey);

      string extraItemName = "Technical support";
      int extraItemQuantity = 3;
      decimal extraItemUnitPrice = 50.0m;
      InvoiceItem extraItem = new InvoiceItem(extraItemName, extraItemQuantity, extraItemUnitPrice);
      clonedTemplate.AddDefaultItem(extraItem);

      PrintTemplateInfo(clonedTemplate);
      Console.WriteLine();

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

      PrintInvoiceToConsole(invoice, invoiceExporter);

      string fileName = "invoice-004.txt";
      string filePath = Path.Combine(outputFolderPath, fileName);
      invoiceExporter.SaveToFile(invoice, filePath);

      string savedMessage = "Saved: " + filePath;
      Console.WriteLine(savedMessage);
      Console.WriteLine();
    }

    static void PrintInvoiceToConsole(Invoice invoice, InvoiceTextExporter invoiceExporter)
    {
      string invoiceText = invoiceExporter.BuildText(invoice);
      Console.WriteLine(invoiceText);
    }

    static InvoiceTemplate BuildStandardTemplate()
    {
      string templateName = "Standard services";
      string sellerCompanyName = "OOO Example";
      string sellerTaxId = "4200123456";
      string sellerAddress = "Kemerovo, Lenin St., 1";
      string sellerBankAccount = "40702810123456789012";
      SellerInfo seller = new SellerInfo(
        sellerCompanyName,
        sellerTaxId,
        sellerAddress,
        sellerBankAccount);

      string defaultClientName = "Default client";
      string defaultClientAddress = "Kemerovo";
      string currencyCode = "RUB";
      decimal vatRatePercent = 20.0m;

      InvoiceTemplate template = new InvoiceTemplate();
      template.TemplateName = templateName;
      template.Seller = seller;
      template.DefaultClientName = defaultClientName;
      template.DefaultClientAddress = defaultClientAddress;
      template.DefaultCurrency = currencyCode;
      template.DefaultVatRatePercent = vatRatePercent;

      string firstItemName = "Consulting";
      int firstItemQuantity = 2;
      decimal firstItemUnitPrice = 150.0m;
      InvoiceItem firstItem = new InvoiceItem(firstItemName, firstItemQuantity, firstItemUnitPrice);
      template.AddDefaultItem(firstItem);

      string secondItemName = "Software development";
      int secondItemQuantity = 1;
      decimal secondItemUnitPrice = 800.0m;
      InvoiceItem secondItem = new InvoiceItem(secondItemName, secondItemQuantity, secondItemUnitPrice);
      template.AddDefaultItem(secondItem);

      return template;
    }

    static InvoiceTemplate BuildNoVatTemplate()
    {
      string templateName = "Services without VAT";
      string sellerCompanyName = "OOO Example";
      string sellerTaxId = "4200123456";
      string sellerAddress = "Kemerovo, Lenin St., 1";
      string sellerBankAccount = "40702810123456789012";
      SellerInfo seller = new SellerInfo(
        sellerCompanyName,
        sellerTaxId,
        sellerAddress,
        sellerBankAccount);

      string defaultClientName = "Default client";
      string defaultClientAddress = "Kemerovo";
      string currencyCode = "RUB";
      decimal vatRatePercent = 0.0m;

      InvoiceTemplate template = new InvoiceTemplate();
      template.TemplateName = templateName;
      template.Seller = seller;
      template.DefaultClientName = defaultClientName;
      template.DefaultClientAddress = defaultClientAddress;
      template.DefaultCurrency = currencyCode;
      template.DefaultVatRatePercent = vatRatePercent;

      string itemName = "Support";
      int itemQuantity = 1;
      decimal itemUnitPrice = 300.0m;
      InvoiceItem item = new InvoiceItem(itemName, itemQuantity, itemUnitPrice);
      template.AddDefaultItem(item);

      return template;
    }

    static void PrintRegistryKeys(TemplateRegistry templateRegistry)
    {
      List<string> templateKeys = templateRegistry.GetTemplateKeys();
      int keyCount = templateKeys.Count;

      for (int keyIndex = 0; keyIndex < keyCount; ++keyIndex) {
        string templateKey = templateKeys[keyIndex];
        Console.WriteLine("  - " + templateKey);
      }
    }

    static void PrintTemplateInfo(InvoiceTemplate template)
    {
      Console.WriteLine("Name: " + template.TemplateName);
      Console.WriteLine("Seller: " + template.Seller.CompanyName);
      Console.WriteLine("Currency: " + template.DefaultCurrency);
      Console.WriteLine("VAT rate: " + template.DefaultVatRatePercent + "%");
      Console.WriteLine("Default client: " + template.DefaultClientName);
      Console.WriteLine("Default items:");

      int itemCount = template.DefaultItems.Count;
      for (int itemIndex = 0; itemIndex < itemCount; ++itemIndex) {
        InvoiceItem currentItem = template.DefaultItems[itemIndex];
        decimal lineTotal = currentItem.GetLineTotal();
        string itemLine = "  - " + currentItem.Name + ": "
          + currentItem.Quantity + " x "
          + currentItem.UnitPrice + " = "
          + lineTotal;
        Console.WriteLine(itemLine);
      }
    }
  }
}
