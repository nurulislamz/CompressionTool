namespace CompressionTool
{
  public interface ICompressor
  {
    public string Run(string[] text);
  }

  public class Compressor : ICompressor
  {
    private readonly IFileOpener _fileOpener;
    private readonly IParser _parser;

    public string Compressor(IFileOpener FileOpener, IParser Parser)
    {
      _fileOpener = FileOpener;
      _parser = Parser;
    }

    public string Run(string[] path)
    {
      string text = _fileOpener.OpenFile(_path) ?? throw new Exception("File not found");

    }
  }
}