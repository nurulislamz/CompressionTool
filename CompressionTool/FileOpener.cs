using System;

namespace CompressionTool
{
  public interface IFileHandler
  {
    public string OpenFile(string path);
  }

  public class FileHandler : IFileHandler
  {

    public string OpenFile(string path)
    {
      if (!File.Exists(path))
      {
        throw new FileNotFoundException("File not found");
      }
      return File.ReadAllText(path);
    }
  }
}