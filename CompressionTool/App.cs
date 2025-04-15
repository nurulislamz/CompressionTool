using System.IO;

namespace CompressionTool
{
  public class App
  {
    private readonly IArgumentParser _argumentParser;
    private readonly ICompress _compress;

    public App(IArgumentParser argumentParser, ICompress compress)
    {
      _argumentParser = argumentParser;
      _compress = compress;
    }

    public async void Run(string[] args)
    {
      var result = await _argumentParser.ParseCommandLine(args);
      string text = File.ReadAllText(result.FilePath);
      if (result.Mode == ModeOptions.Compress)
      {
        _compress.SaveCompressedFile(text, "output.txt");
      }
    }
  }
}