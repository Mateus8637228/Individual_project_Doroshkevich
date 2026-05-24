using Individual_project.Models;
using Individual_project.Services;

namespace Individual_project.Factories
{
  public static class TemplateFactory
  {
    public static void RegisterDefaultTemplates(InvoiceGenerator invoiceGenerator)
    {
      string standardTemplateKey = "standard";
      string noVatTemplateKey = "no-vat";

      InvoiceTemplate standardTemplate = BuildStandardTemplate();
      InvoiceTemplate noVatTemplate = BuildNoVatTemplate();

      invoiceGenerator.RegisterTemplate(standardTemplateKey, standardTemplate);
      invoiceGenerator.RegisterTemplate(noVatTemplateKey, noVatTemplate);
    }

    public static InvoiceTemplate BuildStandardTemplate()
    {
      string templateName = "Standard services";
      SellerInfo seller = BuildDefaultSeller();

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

    public static InvoiceTemplate BuildNoVatTemplate()
    {
      string templateName = "Services without VAT";
      SellerInfo seller = BuildDefaultSeller();

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

    private static SellerInfo BuildDefaultSeller()
    {
      string sellerCompanyName = "OOO Example";
      string sellerTaxId = "4200123456";
      string sellerAddress = "Kemerovo, Lenin St., 1";
      string sellerBankAccount = "40702810123456789012";
      SellerInfo seller = new SellerInfo(
        sellerCompanyName,
        sellerTaxId,
        sellerAddress,
        sellerBankAccount);

      return seller;
    }
  }
}
