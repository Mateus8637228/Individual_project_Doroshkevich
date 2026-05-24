using System.Collections.Generic;
using Individual_project.Models;
using Individual_project.Services;

namespace Individual_project.UI
{
  public class ConsolePrinter
  {
    private InvoiceTextExporter invoiceExporter;

    public ConsolePrinter(InvoiceTextExporter invoiceExporter)
    {
      this.invoiceExporter = invoiceExporter;
    }

    public void PrintRegistryKeys(TemplateRegistry templateRegistry)
    {
      List<string> templateKeys = templateRegistry.GetTemplateKeys();
      int keyCount = templateKeys.Count;

      for (int keyIndex = 0; keyIndex < keyCount; ++keyIndex) {
        string templateKey = templateKeys[keyIndex];
        System.Console.WriteLine("  - " + templateKey);
      }
    }

    public void PrintTemplateInfo(InvoiceTemplate template)
    {
      System.Console.WriteLine("Name: " + template.TemplateName);
      System.Console.WriteLine("Seller: " + template.Seller.CompanyName);
      System.Console.WriteLine("Currency: " + template.DefaultCurrency);
      System.Console.WriteLine("VAT rate: " + template.DefaultVatRatePercent + "%");
      System.Console.WriteLine("Default client: " + template.DefaultClientName);
      System.Console.WriteLine("Default items:");

      int itemCount = template.DefaultItems.Count;
      for (int itemIndex = 0; itemIndex < itemCount; ++itemIndex) {
        InvoiceItem currentItem = template.DefaultItems[itemIndex];
        decimal lineTotal = currentItem.GetLineTotal();
        string itemLine = "  - " + currentItem.Name + ": "
          + currentItem.Quantity + " x "
          + currentItem.UnitPrice + " = "
          + lineTotal;
        System.Console.WriteLine(itemLine);
      }
    }

    public void PrintInvoice(Invoice invoice)
    {
      string invoiceText = invoiceExporter.BuildText(invoice);
      System.Console.WriteLine(invoiceText);
    }
  }
}
