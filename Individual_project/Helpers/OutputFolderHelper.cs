using System.IO;

namespace Individual_project.Helpers
{
  public static class OutputFolderHelper
  {
    public static string PrepareOutputFolder()
    {
      string outputFolderName = "output";
      string outputFolderPath = Path.Combine(Directory.GetCurrentDirectory(), outputFolderName);

      if (!Directory.Exists(outputFolderPath)) {
        Directory.CreateDirectory(outputFolderPath);
      }

      return outputFolderPath;
    }
  }
}
