using System;
using System.Collections.Generic;

namespace Individual_project.Models
{
  public class Invoice
  {
    public string Number { get; set; }
    public DateTime IssueDate { get; set; }
    public SellerInfo Seller { get; set; }
    public string ClientName { get; set; }
    public string ClientAddress { get; set; }
    public string Currency { get; set; }
    public decimal VatRatePercent { get; set; }
    public List<InvoiceItem> Items { get; }

    public Invoice()
    {
      Items = new List<InvoiceItem>();
      IssueDate = DateTime.Today;

      string defaultCurrency = "RUB";
      Currency = defaultCurrency;
    }

    public void AddItem(InvoiceItem item)
    {
      if (item == null) {
        throw new ArgumentNullException(nameof(item));
      }

      Items.Add(item);
    }

    public decimal GetSubtotal()
    {
      decimal subtotal = 0.0m;
      int itemCount = Items.Count;

      for (int itemIndex = 0; itemIndex < itemCount; ++itemIndex) {
        InvoiceItem currentItem = Items[itemIndex];
        decimal lineTotal = currentItem.GetLineTotal();
        subtotal = subtotal + lineTotal;
      }

      return subtotal;
    }

    public decimal GetVatAmount()
    {
      decimal subtotal = GetSubtotal();
      decimal vatRatePercent = VatRatePercent;
      decimal percentBase = 100.0m;
      decimal vatRatio = vatRatePercent / percentBase;
      decimal vatBeforeRound = subtotal * vatRatio;

      int moneyDecimalPlaces = 2;
      decimal vatAmount = decimal.Round(vatBeforeRound, moneyDecimalPlaces);

      return vatAmount;
    }

    public decimal GetTotal()
    {
      decimal subtotal = GetSubtotal();
      decimal vatAmount = GetVatAmount();
      decimal total = subtotal + vatAmount;

      return total;
    }
  }
}
