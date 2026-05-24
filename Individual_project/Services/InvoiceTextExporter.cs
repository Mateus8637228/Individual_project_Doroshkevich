using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Individual_project.Models;

namespace Individual_project.Services
{
  public class InvoiceTextExporter
  {
    public string BuildText(Invoice invoice)
    {
      if (invoice == null) {
        throw new ArgumentNullException(nameof(invoice));
      }

      List<string> lines = new List<string>();
      int moneyDecimalPlaces = 2;
      string moneyFormat = "F" + moneyDecimalPlaces.ToString();

      lines.Add("=== INVOICE ===");
      lines.Add("Number: " + invoice.Number);
      lines.Add("Date: " + invoice.IssueDate.ToString("dd.MM.yyyy"));
      lines.Add("");
      lines.Add("Seller: " + invoice.Seller.CompanyName);
      lines.Add("INN: " + invoice.Seller.TaxId);
      lines.Add("Address: " + invoice.Seller.Address);
      lines.Add("");
      lines.Add("Client: " + invoice.ClientName);
      lines.Add("Address: " + invoice.ClientAddress);
      lines.Add("");
      lines.Add("Items:");

      int itemCount = invoice.Items.Count;
      for (int itemIndex = 0; itemIndex < itemCount; ++itemIndex) {
        InvoiceItem currentItem = invoice.Items[itemIndex];
        decimal lineTotal = currentItem.GetLineTotal();
        string itemLine = "  - " + currentItem.Name + ": "
          + currentItem.Quantity + " x "
          + currentItem.UnitPrice.ToString(moneyFormat) + " = "
          + lineTotal.ToString(moneyFormat) + " " + invoice.Currency;
        lines.Add(itemLine);
      }

      lines.Add("");
      decimal subtotal = invoice.GetSubtotal();
      decimal vatAmount = invoice.GetVatAmount();
      decimal total = invoice.GetTotal();

      lines.Add("Subtotal: " + subtotal.ToString(moneyFormat) + " " + invoice.Currency);
      lines.Add("VAT (" + invoice.VatRatePercent + "%): "
        + vatAmount.ToString(moneyFormat) + " " + invoice.Currency);
      lines.Add("TOTAL: " + total.ToString(moneyFormat) + " " + invoice.Currency);

      string resultText = JoinLines(lines);

      return resultText;
    }

    public void SaveToFile(Invoice invoice, string filePath)
    {
      if (filePath == null) {
        throw new ArgumentNullException(nameof(filePath));
      }

      string invoiceText = BuildText(invoice);
      File.WriteAllText(filePath, invoiceText, Encoding.UTF8);
    }

    private string JoinLines(List<string> lines)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int lineCount = lines.Count;

      for (int lineIndex = 0; lineIndex < lineCount; ++lineIndex) {
        string currentLine = lines[lineIndex];
        stringBuilder.AppendLine(currentLine);
      }

      string resultText = stringBuilder.ToString();

      return resultText;
    }
  }
}
