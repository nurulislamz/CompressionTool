namespace CompressionTool
{
  public class App
  {
    private readonly IFileOpener _fileOpener;
    private readonly IArgumentParser _argumentParser;
    private readonly IFrequencyCounter _frequencyCounter;
    private readonly IPriorityQueue _priorityQueue;

    public App(IFileOpener FileOpener, IArgumentParser ArgumentParser, IFrequencyCounter FrequencyCounter, IPriorityQueue PriorityQueue)
    {
      _fileOpener = FileOpener;
      _argumentParser = ArgumentParser;
      _frequencyCounter = FrequencyCounter;
      _priorityQueue = PriorityQueue;
    }

    public async void Run(string[] args)
    {
      var result = await _argumentParser.ParseCommandLine(args);
      string text = _fileOpener.OpenFile(result.FilePath);
      if (result.Mode == ModeOptions.Compress)
      {
        var frequencyMap = _frequencyCounter.CountFrequency(text);

        foreach ((char character, int frequency) in frequencyMap)
        {
          _priorityQueue.Push(character, frequency);
        }
      }
    }
  }
}