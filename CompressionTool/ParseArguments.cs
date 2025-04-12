using System;
using System.CommandLine;

namespace CompressionTool
{
  public enum ModeOptions
  {
    Compress,
    Decompress
  }

  public class ParseResult
  {
    public required string FilePath { get; init; }
    public required ModeOptions Mode { get; init; }
  }

  public interface IParseArguments
  {
    public Task<ParseResult> ParseCommandLine(string[] args);
  }

  public class ParseArguments : IParseArguments
  {
    public async Task<ParseResult> ParseCommandLine(string[] args)
    {
      var fileOption = new Option<FileInfo?>(
        name: "-f",
        description: "The file to process"
      );

      var modeOption = new Option<string>(
        name: "-m",
        description: "Compress Mode of operation: compress or decompress",
        getDefaultValue: () => "compress"
      ).FromAmong("Compress", "Decompress");

      var rootCommand = new RootCommand("Compression Tool");
      rootCommand.AddOption(fileOption);
      rootCommand.AddOption(modeOption);

      ParseResult result = null;

      rootCommand.SetHandler((FileInfo? file, string mode) =>
      {
        if (file == null) throw new ArgumentNullException(nameof(file));
        string filePath = file.FullName.Replace('\\', '/');

        ModeOptions chosenMode = mode switch
        {
          "Compress" => ModeOptions.Compress,
          "Decompress" => ModeOptions.Decompress,
          _ => throw new ArgumentException("Incorrect Mode Option")
        };

        result = new ParseResult
        {
          FilePath = filePath,
          Mode = chosenMode
        };
      },
      fileOption, modeOption);

      await rootCommand.InvokeAsync(args);
      return result ?? throw new Exception("Failed to parse arguments");
    }
  }
}