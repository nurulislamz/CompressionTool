using System.IO;

namespace CompressionTool
{
  public class App
  {
    private readonly IArgumentParser _argumentParser;
    private readonly IFrequencyCounter _frequencyCounter;
    private readonly IPriorityQueue _priorityQueue;
    private readonly HuffmanTree _huffmanTree;

    public App(IArgumentParser argumentParser, IFrequencyCounter frequencyCounter, IPriorityQueue priorityQueue,
      HuffmanTree huffmanTree)
    {
      _argumentParser = argumentParser;
      _frequencyCounter = frequencyCounter;
      _priorityQueue = priorityQueue;
      _huffmanTree = huffmanTree;
    }

    public async void Run(string[] args)
    {
      var result = await _argumentParser.ParseCommandLine(args);
      string text = File.ReadAllText(result.FilePath);
      if (result.Mode == ModeOptions.Compress)
      {
        var frequencyMap = _frequencyCounter.CountFrequency(text);
        CreatePriorityQueue(frequencyMap);
        _huffmanTree.BuildHuffmanTreeFromPriorityQueue(_priorityQueue);
        _huffmanTree.GetEncodingMapAsString();
      }
    }

    public void CreatePriorityQueue(Dictionary<char, int> frequencyMap)
    {
      foreach ((char character, int frequency) in frequencyMap)
      {
        _priorityQueue.Push(new HuffmanNode(character, frequency));
      }
    }
  }
}