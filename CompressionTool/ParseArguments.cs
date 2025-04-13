using System;
using System.CommandLine;

namespace CompressionTool
{
  public enum ModeOptions
  {
    Compress,
    Decompress
  }

  public class ParsedResult
  {
    public required string FilePath { get; init; }
    public required ModeOptions Mode { get; init; }
  }

  public interface IArgumentParser 
  {
    public Task<ParsedResult> ParseCommandLine(string[] args);
  }

  public class ArgumentParser : IArgumentParser
  {
    public async Task<ParsedResult> ParseCommandLine(string[] args)
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

      ParsedResult result = null;

      rootCommand.SetHandler((FileInfo? file, string mode) =>
      {
        if (file == null || file.Exists == false) throw new FileNotFoundException(nameof(file));
        string filePath = file.FullName.Replace('\\', '/');

        ModeOptions chosenMode = mode switch
        {
          "Compress" => ModeOptions.Compress,
          "Decompress" => ModeOptions.Decompress,
          _ => throw new ArgumentException("Incorrect Mode Option")
        };

        result = new ParsedResult
        {
          FilePath = filePath,
          Mode = chosenMode
        };
      },
      fileOption, modeOption);

      await rootCommand.InvokeAsync(args);
      return result ?? throw new ArgumentException("Failed to parse arguments");
    }
  }
}