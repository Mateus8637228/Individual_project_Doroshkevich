using System;
using System.Globalization;

namespace Individual_project.UI
{
  public class ConsoleInputReader
  {
    public string ReadRequiredLine(string prompt)
    {
      string value = "";

      while (value.Length == 0) {
        Console.Write(prompt);
        string inputLine = Console.ReadLine();

        if (inputLine != null) {
          value = inputLine.Trim();
        }

        if (value.Length == 0) {
          Console.WriteLine("Value cannot be empty. Try again.");
        }
      }

      return value;
    }

    public string ReadLineWithDefault(string prompt, string defaultValue)
    {
      Console.Write(prompt + " [" + defaultValue + "]: ");
      string inputLine = Console.ReadLine();
      string value = defaultValue;

      if (inputLine != null) {
        string trimmedInput = inputLine.Trim();
        if (trimmedInput.Length > 0) {
          value = trimmedInput;
        }
      }

      return value;
    }

    public int ReadIntInRange(string prompt, int minValue, int maxValue)
    {
      bool isValid = false;
      int resultValue = minValue;

      while (!isValid) {
        Console.Write(prompt);
        string inputLine = Console.ReadLine();

        int parsedValue = 0;
        bool isParsed = int.TryParse(inputLine, out parsedValue);

        if (isParsed) {
          if (parsedValue >= minValue && parsedValue <= maxValue) {
            resultValue = parsedValue;
            isValid = true;
          } else {
            Console.WriteLine("Enter a number from " + minValue + " to " + maxValue + ".");
          }
        } else {
          Console.WriteLine("Enter a valid integer.");
        }
      }

      return resultValue;
    }

    public decimal ReadDecimal(string prompt)
    {
      bool isValid = false;
      decimal resultValue = 0.0m;

      while (!isValid) {
        Console.Write(prompt);
        string inputLine = Console.ReadLine();

        decimal parsedValue = 0.0m;
        bool isParsed = decimal.TryParse(
          inputLine,
          NumberStyles.Number,
          CultureInfo.InvariantCulture,
          out parsedValue);

        if (isParsed) {
          resultValue = parsedValue;
          isValid = true;
        } else {
          Console.WriteLine("Enter a valid number (example: 150.0).");
        }
      }

      return resultValue;
    }

    public DateTime ReadDate(string prompt)
    {
      bool isValid = false;
      DateTime resultDate = DateTime.Today;

      while (!isValid) {
        Console.Write(prompt + " (dd.MM.yyyy): ");
        string inputLine = Console.ReadLine();

        DateTime parsedDate = DateTime.Today;
        bool isParsed = DateTime.TryParseExact(
          inputLine,
          "dd.MM.yyyy",
          CultureInfo.InvariantCulture,
          DateTimeStyles.None,
          out parsedDate);

        if (isParsed) {
          resultDate = parsedDate;
          isValid = true;
        } else {
          Console.WriteLine("Enter date in format dd.MM.yyyy.");
        }
      }

      return resultDate;
    }

    public int ReadMenuChoice(int minChoice, int maxChoice)
    {
      string prompt = "Your choice (" + minChoice + "-" + maxChoice + "): ";
      int choice = ReadIntInRange(prompt, minChoice, maxChoice);

      return choice;
    }
  }
}
