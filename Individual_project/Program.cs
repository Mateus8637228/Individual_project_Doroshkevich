using System;
using Individual_project.Models;

namespace Individual_project
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      InvoiceTemplate standardTemplate = BuildStandardTemplate();

      string templateTitle = "=== INVOICE TEMPLATE ===";
      Console.WriteLine(templateTitle);
      PrintTemplateInfo(standardTemplate);
      Console.WriteLine();

      string firstInvoiceNumber = "INV-001";
      int firstIssueYear = 2026;
      int firstIssueMonth = 5;
      int firstIssueDay = 24;
      DateTime firstIssueDate = new DateTime(firstIssueYear, firstIssueMonth, firstIssueDay);

      string firstClientName = "Ivanov IE";
      string firstClientAddress = "Minsk, Client St., 10";
      Invoice firstInvoice = standardTemplate.CreateInvoice(
        firstInvoiceNumber,
        firstIssueDate,
        firstClientName,
        firstClientAddress);

      PrintInvoice(firstInvoice);
      Console.WriteLine();

      string secondInvoiceNumber = "INV-002";
      int secondIssueDay = 25;
      DateTime secondIssueDate = new DateTime(firstIssueYear, firstIssueMonth, secondIssueDay);

      string secondClientName = "Petrov IE";
      string secondClientAddress = "Minsk, Trade St., 5";
      Invoice secondInvoice = standardTemplate.CreateInvoice(
        secondInvoiceNumber,
        secondIssueDate,
        secondClientName,
        secondClientAddress);

      PrintInvoice(secondInvoice);

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
