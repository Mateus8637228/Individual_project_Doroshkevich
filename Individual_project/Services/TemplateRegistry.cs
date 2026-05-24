using System;
using System.Collections.Generic;
using Individual_project.Models;
using Individual_project.Prototype;

namespace Individual_project.Services
{
  public class TemplateRegistry
  {
    private Dictionary<string, InvoiceTemplate> templates;

    public TemplateRegistry()
    {
      templates = new Dictionary<string, InvoiceTemplate>();
    }

    public void Register(string templateKey, InvoiceTemplate template)
    {
      if (templateKey == null) {
        throw new ArgumentNullException(nameof(templateKey));
      }

      if (template == null) {
        throw new ArgumentNullException(nameof(template));
      }

      templates[templateKey] = template;
    }

    public bool Contains(string templateKey)
    {
      if (templateKey == null) {
        throw new ArgumentNullException(nameof(templateKey));
      }

      bool isFound = templates.ContainsKey(templateKey);

      return isFound;
    }

    public InvoiceTemplate GetClone(string templateKey)
    {
      if (templateKey == null) {
        throw new ArgumentNullException(nameof(templateKey));
      }

      if (!templates.ContainsKey(templateKey)) {
        string errorMessage = "Template not found: " + templateKey;
        throw new KeyNotFoundException(errorMessage);
      }

      InvoiceTemplate originalTemplate = templates[templateKey];
      IPrototype prototypeReference = originalTemplate;
      IPrototype clonedPrototype = prototypeReference.Clone();
      InvoiceTemplate clonedTemplate = (InvoiceTemplate)clonedPrototype;

      return clonedTemplate;
    }

    public int GetTemplateKeyCount()
    {
      int templateKeyCount = templates.Count;

      return templateKeyCount;
    }

    public List<string> GetTemplateKeys()
    {
      List<string> templateKeys = new List<string>(templates.Keys);
      int keyCount = templateKeys.Count;

      for (int outerKeyIndex = 0; outerKeyIndex < keyCount; ++outerKeyIndex) {
        for (int innerKeyIndex = outerKeyIndex + 1; innerKeyIndex < keyCount; ++innerKeyIndex) {
          string leftKey = templateKeys[outerKeyIndex];
          string rightKey = templateKeys[innerKeyIndex];

          if (string.Compare(leftKey, rightKey) > 0) {
            templateKeys[outerKeyIndex] = rightKey;
            templateKeys[innerKeyIndex] = leftKey;
          }
        }
      }

      return templateKeys;
    }
  }
}
