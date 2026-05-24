using System;
using Individual_project.Models;

namespace Individual_project.Services
{
  public class InvoiceGenerator
  {
    private TemplateRegistry templateRegistry;

    public InvoiceGenerator()
    {
      templateRegistry = new TemplateRegistry();
    }

    public InvoiceGenerator(TemplateRegistry registry)
    {
      if (registry == null) {
        throw new ArgumentNullException(nameof(registry));
      }

      templateRegistry = registry;
    }

    public TemplateRegistry GetTemplateRegistry()
    {
      return templateRegistry;
    }

    public void RegisterTemplate(string templateKey, InvoiceTemplate template)
    {
      templateRegistry.Register(templateKey, template);
    }

    public Invoice GenerateInvoice(
      string templateKey,
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

      InvoiceTemplate workingTemplate = templateRegistry.GetClone(templateKey);
      Invoice invoice = workingTemplate.CreateInvoice(
        invoiceNumber,
        issueDate,
        clientName,
        clientAddress);

      return invoice;
    }

    public InvoiceTemplate GetClonedTemplate(string templateKey)
    {
      InvoiceTemplate clonedTemplate = templateRegistry.GetClone(templateKey);

      return clonedTemplate;
    }
  }
}
