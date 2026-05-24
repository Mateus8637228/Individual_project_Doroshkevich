using System;
using System.Collections.Generic;

namespace Individual_project.Models
{
  public class InvoiceTemplate
  {
    public string TemplateName { get; set; }
    public SellerInfo Seller { get; set; }
    public string DefaultCurrency { get; set; }
    public decimal DefaultVatRatePercent { get; set; }
    public string DefaultClientName { get; set; }
    public string DefaultClientAddress { get; set; }
    public List<InvoiceItem> DefaultItems { get; }

    public InvoiceTemplate()
    {
      DefaultItems = new List<InvoiceItem>();

      string defaultCurrency = "BYN";
      DefaultCurrency = defaultCurrency;

      decimal zeroVatRate = 0.0m;
      DefaultVatRatePercent = zeroVatRate;
    }

    public void AddDefaultItem(InvoiceItem item)
    {
      if (item == null) {
        throw new ArgumentNullException(nameof(item));
      }

      DefaultItems.Add(item);
    }

    public Invoice CreateInvoice(string invoiceNumber, DateTime issueDate)
    {
      string clientName = DefaultClientName;
      string clientAddress = DefaultClientAddress;

      return CreateInvoice(invoiceNumber, issueDate, clientName, clientAddress);
    }

    public Invoice CreateInvoice(
      string invoiceNumber,
      DateTime issueDate,
      string clientName,
      string clientAddress)
    {
      if (invoiceNumber == null) {
        throw new ArgumentNullException(nameof(invoiceNumber));
      }

      if (clientName == null) {
        throw new ArgumentNullException(nameof(clientName));
      }

      if (clientAddress == null) {
        throw new ArgumentNullException(nameof(clientAddress));
      }

      Invoice invoice = new Invoice();
      invoice.Number = invoiceNumber;
      invoice.IssueDate = issueDate;
      invoice.Seller = Seller;
      invoice.ClientName = clientName;
      invoice.ClientAddress = clientAddress;
      invoice.Currency = DefaultCurrency;
      invoice.VatRatePercent = DefaultVatRatePercent;

      int itemCount = DefaultItems.Count;
      for (int itemIndex = 0; itemIndex < itemCount; ++itemIndex) {
        InvoiceItem sourceItem = DefaultItems[itemIndex];

        string itemName = sourceItem.Name;
        int itemQuantity = sourceItem.Quantity;
        decimal itemUnitPrice = sourceItem.UnitPrice;

        InvoiceItem copiedItem = new InvoiceItem(itemName, itemQuantity, itemUnitPrice);
        invoice.AddItem(copiedItem);
      }

      return invoice;
    }
  }
}
