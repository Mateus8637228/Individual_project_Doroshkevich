using System;

namespace Individual_project.Models
{
  public class InvoiceItem
  {
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public InvoiceItem()
    {
    }

    public InvoiceItem(string name, int quantity, decimal unitPrice)
    {
      if (name == null) {
        throw new ArgumentNullException(nameof(name));
      }

      int minQuantity = 0;
      if (quantity <= minQuantity) {
        throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
      }

      decimal minPrice = 0.0m;
      if (unitPrice < minPrice) {
        throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
      }

      Name = name;
      Quantity = quantity;
      UnitPrice = unitPrice;
    }

    public decimal GetLineTotal()
    {
      int quantity = Quantity;
      decimal unitPrice = UnitPrice;
      decimal lineTotal = quantity * unitPrice;

      return lineTotal;
    }
  }
}
