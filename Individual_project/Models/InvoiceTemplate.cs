using System;
using System.Collections.Generic;
using Individual_project.Prototype;

namespace Individual_project.Models
{
  public class InvoiceTemplate : IPrototype
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

      string defaultCurrency = "RUB";
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

    public IPrototype Clone()
    {
      InvoiceTemplate copy = new InvoiceTemplate();

      copy.TemplateName = TemplateName;
      copy.DefaultCurrency = DefaultCurrency;
      copy.DefaultVatRatePercent = DefaultVatRatePercent;
      copy.DefaultClientName = DefaultClientName;
      copy.DefaultClientAddress = DefaultClientAddress;

      if (Seller != null) {
        copy.Seller = Seller.Clone();
      }

      CopyDefaultItemsTo(copy);

      return copy;
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
      invoice.ClientName = clientName;
      invoice.ClientAddress = clientAddress;
      invoice.Currency = DefaultCurrency;
      invoice.VatRatePercent = DefaultVatRatePercent;

      if (Seller != null) {
        invoice.Seller = Seller.Clone();
      }

      CopyDefaultItemsToInvoice(invoice);

      return invoice;
    }

    private InvoiceItem CopyItem(InvoiceItem sourceItem)
    {
      string itemName = sourceItem.Name;
      int itemQuantity = sourceItem.Quantity;
      decimal itemUnitPrice = sourceItem.UnitPrice;
      InvoiceItem copiedItem = new InvoiceItem(itemName, itemQuantity, itemUnitPrice);

      return copiedItem;
    }

    private void CopyDefaultItemsTo(InvoiceTemplate targetTemplate)
    {
      if (targetTemplate == null) {
        throw new ArgumentNullException(nameof(targetTemplate));
      }

      int itemCount = DefaultItems.Count;
      for (int itemIndex = 0; itemIndex < itemCount; ++itemIndex) {
        InvoiceItem sourceItem = DefaultItems[itemIndex];
        InvoiceItem copiedItem = CopyItem(sourceItem);
        targetTemplate.AddDefaultItem(copiedItem);
      }
    }

    private void CopyDefaultItemsToInvoice(Invoice invoice)
    {
      if (invoice == null) {
        throw new ArgumentNullException(nameof(invoice));
      }

      int itemCount = DefaultItems.Count;
      for (int itemIndex = 0; itemIndex < itemCount; ++itemIndex) {
        InvoiceItem sourceItem = DefaultItems[itemIndex];
        InvoiceItem copiedItem = CopyItem(sourceItem);
        invoice.AddItem(copiedItem);
      }
    }
  }
}
