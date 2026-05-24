namespace Individual_project.Models
{
  public class SellerInfo
  {
    public string CompanyName { get; set; }
    public string TaxId { get; set; }
    public string Address { get; set; }
    public string BankAccount { get; set; }

    public SellerInfo()
    {
    }

    public SellerInfo(string companyName, string taxId, string address, string bankAccount)
    {
      CompanyName = companyName;
      TaxId = taxId;
      Address = address;
      BankAccount = bankAccount;
    }

    public SellerInfo Clone()
    {
      string companyName = CompanyName;
      string taxId = TaxId;
      string address = Address;
      string bankAccount = BankAccount;
      SellerInfo copy = new SellerInfo(companyName, taxId, address, bankAccount);

      return copy;
    }
  }
}
