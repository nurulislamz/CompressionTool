
namespace CompressionTool {
  public interface IParser
  {
    public Dictionary<char, int> Parse();
  }

  public class Parser : IParser
  {
    private readonly string _fileContents;
    private Dictionary<char, int> frequency;

    public Parser(string fileContents)
    {
      _fileContents = fileContents;
      frequency = new Dictionary<char, int>();
    }

    public Dictionary<char, int> Parse()
    {
      foreach (var character in _fileContents)
      {
        if (frequency.ContainsKey(character))
        {
          frequency[character]++;
        }
        else
        {
          frequency.Add(character, 1);
        }
      }
      return frequency;
    }
  }
}