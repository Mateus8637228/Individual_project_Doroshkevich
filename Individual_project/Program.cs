using System;
using Individual_project.Models;

namespace Individual_project
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      string invoiceNumber = "INV-001";
      int issueYear = 2026;
      int issueMonth = 5;
      int issueDay = 24;
      DateTime issueDate = new DateTime(issueYear, issueMonth, issueDay);

      string sellerCompanyName = "Example LLC";
      string sellerTaxId = "123456789";
      string sellerAddress = "Minsk, Example St., 1";
      string sellerBankAccount = "BY00BANK00000000000000";
      SellerInfo seller = new SellerInfo(sellerCompanyName, sellerTaxId, sellerAddress, sellerBankAccount);

      string clientName = "Ivanov IE";
      string clientAddress = "Minsk, Client St., 10";
      string currencyCode = "BYN";
      decimal vatRatePercent = 20.0m;

      Invoice invoice = new Invoice();
      invoice.Number = invoiceNumber;
      invoice.IssueDate = issueDate;
      invoice.Seller = seller;
      invoice.ClientName = clientName;
      invoice.ClientAddress = clientAddress;
      invoice.Currency = currencyCode;
      invoice.VatRatePercent = vatRatePercent;

      string firstItemName = "Consulting";
      int firstItemQuantity = 2;
      decimal firstItemUnitPrice = 150.0m;
      InvoiceItem firstItem = new InvoiceItem(firstItemName, firstItemQuantity, firstItemUnitPrice);
      invoice.AddItem(firstItem);

      string secondItemName = "Software development";
      int secondItemQuantity = 1;
      decimal secondItemUnitPrice = 800.0m;
      InvoiceItem secondItem = new InvoiceItem(secondItemName, secondItemQuantity, secondItemUnitPrice);
      invoice.AddItem(secondItem);

      PrintInvoice(invoice);

      Console.WriteLine();
      Console.WriteLine("Press Enter to exit...");
      Console.ReadLine();
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
