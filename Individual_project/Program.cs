using System;
using System.Collections.Generic;
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
      TemplateRegistry templateRegistry = invoiceGenerator.GetTemplateRegistry();

      string standardTemplateKey = "standard";
      string noVatTemplateKey = "no-vat";

      InvoiceTemplate standardTemplate = BuildStandardTemplate();
      InvoiceTemplate noVatTemplate = BuildNoVatTemplate();

      invoiceGenerator.RegisterTemplate(standardTemplateKey, standardTemplate);
      invoiceGenerator.RegisterTemplate(noVatTemplateKey, noVatTemplate);

      string registryTitle = "=== TEMPLATE REGISTRY ===";
      Console.WriteLine(registryTitle);
      PrintRegistryKeys(templateRegistry);
      Console.WriteLine();

      int issueYear = 2026;
      int issueMonth = 5;
      int issueDay = 24;
      DateTime issueDate = new DateTime(issueYear, issueMonth, issueDay);

      string firstInvoiceNumber = "INV-001";
      string firstClientName = "Ivanov IE";
      string firstClientAddress = "Minsk, Client St., 10";
      Invoice firstInvoice = invoiceGenerator.GenerateInvoice(
        standardTemplateKey,
        firstInvoiceNumber,
        issueDate,
        firstClientName,
        firstClientAddress);
      PrintInvoice(firstInvoice);
      Console.WriteLine();

      string secondInvoiceNumber = "INV-002";
      string secondClientName = "Petrov IE";
      string secondClientAddress = "Minsk, Trade St., 5";
      Invoice secondInvoice = invoiceGenerator.GenerateInvoice(
        noVatTemplateKey,
        secondInvoiceNumber,
        issueDate,
        secondClientName,
        secondClientAddress);
      PrintInvoice(secondInvoice);
      Console.WriteLine();

      string cloneDemoTitle = "=== CLONE FROM REGISTRY AND MODIFY ===";
      Console.WriteLine(cloneDemoTitle);
      InvoiceTemplate clonedTemplate = invoiceGenerator.GetClonedTemplate(standardTemplateKey);

      string modifiedTemplateName = "Standard services (custom)";
      clonedTemplate.TemplateName = modifiedTemplateName;

      decimal customVatRate = 10.0m;
      clonedTemplate.DefaultVatRatePercent = customVatRate;

      PrintTemplateInfo(clonedTemplate);
      Console.WriteLine();

      string thirdInvoiceNumber = "INV-003";
      string thirdClientName = "Sidorov IE";
      string thirdClientAddress = "Minsk, Business St., 3";
      Invoice thirdInvoice = clonedTemplate.CreateInvoice(
        thirdInvoiceNumber,
        issueDate,
        thirdClientName,
        thirdClientAddress);
      PrintInvoice(thirdInvoice);

      Console.WriteLine();
      Console.WriteLine("Press Enter to exit...");
      Console.ReadLine();
    }

    static InvoiceTemplate BuildStandardTemplate()
    {
      string templateName = "Standard services";
      string sellerCompanyName = "Example LLC";
      string sellerTaxId = "123456789";
      string sellerAddress = "Minsk, Example St., 1";
      string sellerBankAccount = "BY00BANK00000000000000";
      SellerInfo seller = new SellerInfo(
        sellerCompanyName,
        sellerTaxId,
        sellerAddress,
        sellerBankAccount);

      string defaultClientName = "Default client";
      string defaultClientAddress = "Minsk";
      string currencyCode = "BYN";
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
      string sellerCompanyName = "Example LLC";
      string sellerTaxId = "123456789";
      string sellerAddress = "Minsk, Example St., 1";
      string sellerBankAccount = "BY00BANK00000000000000";
      SellerInfo seller = new SellerInfo(
        sellerCompanyName,
        sellerTaxId,
        sellerAddress,
        sellerBankAccount);

      string defaultClientName = "Default client";
      string defaultClientAddress = "Minsk";
      string currencyCode = "BYN";
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

    static void PrintInvoice(Invoice invoice)
    {
      int moneyDecimalPlaces = 2;
      string moneyFormat = "F" + moneyDecimalPlaces.ToString();

      Console.WriteLine("=== INVOICE ===");
      Console.WriteLine("Number: " + invoice.Number);
      Console.WriteLine("Date: " + invoice.IssueDate.ToString("dd.MM.yyyy"));
      Console.WriteLine();
      Console.WriteLine("Seller: " + invoice.Seller.CompanyName);
      Console.WriteLine("Tax ID: " + invoice.Seller.TaxId);
      Console.WriteLine("Address: " + invoice.Seller.Address);
      Console.WriteLine();
      Console.WriteLine("Client: " + invoice.ClientName);
      Console.WriteLine("Address: " + invoice.ClientAddress);
      Console.WriteLine();
      Console.WriteLine("Items:");

      int itemCount = invoice.Items.Count;
      for (int itemIndex = 0; itemIndex < itemCount; ++itemIndex) {
        InvoiceItem currentItem = invoice.Items[itemIndex];
        decimal lineTotal = currentItem.GetLineTotal();
        string itemLine = "  - " + currentItem.Name + ": "
          + currentItem.Quantity + " x "
          + currentItem.UnitPrice.ToString(moneyFormat) + " = "
          + lineTotal.ToString(moneyFormat) + " " + invoice.Currency;
        Console.WriteLine(itemLine);
      }

      Console.WriteLine();
      decimal subtotal = invoice.GetSubtotal();
      decimal vatAmount = invoice.GetVatAmount();
      decimal total = invoice.GetTotal();

      Console.WriteLine("Subtotal: " + subtotal.ToString(moneyFormat) + " " + invoice.Currency);
      Console.WriteLine("VAT (" + invoice.VatRatePercent + "%): "
        + vatAmount.ToString(moneyFormat) + " " + invoice.Currency);
      Console.WriteLine("TOTAL: " + total.ToString(moneyFormat) + " " + invoice.Currency);
    }
  }
}
