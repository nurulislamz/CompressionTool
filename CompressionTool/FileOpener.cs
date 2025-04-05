using System;

public interface IFileOpener
{
  public string OpenFile(string path);
}

public class FileOpener : IFileOpener
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