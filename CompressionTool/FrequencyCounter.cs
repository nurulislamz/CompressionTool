namespace CompressionTool {
  public interface IFrequencyCounter
  {
    public Dictionary<char, int> CountFrequency(string fileContents);
  }

  public class FrequencyCounter : IFrequencyCounter
  {
    private readonly Dictionary<char, int> _frequency;

    public FrequencyCounter()
    {
      _frequency = new Dictionary<char, int>();
    }

    public Dictionary<char, int> CountFrequency(string fileContents)
    {
      foreach (var character in fileContents)
      {
        if (_frequency.ContainsKey(character))
        {
          _frequency[character]++;
        }
        else
        {
          _frequency.Add(character, 1);
        }
      }
      return _frequency;
    }
  }
}