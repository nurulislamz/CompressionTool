namespace CompressionTool
{
  public class HuffmanNode
  {
    public char? Character { get; set; }
    public int Frequency { get; set; }
    public HuffmanNode? Left { get; set; }
    public HuffmanNode? Right { get; set; }

    public HuffmanNode(char? character, int frequency)
    {
      Character = character;
      Frequency = frequency;
      Left = null;
      Right = null;
    }
  }

  public class HuffmanTree
  {
    private Dictionary<char, int> _characterFrequency;
    private PriorityQueue priorityQueue;
    private HuffmanNode? root;

    public HuffmanTree(Dictionary<char, int> characterFrequency)
    {
      _characterFrequency = characterFrequency;
      priorityQueue = new PriorityQueue();
    }

    public void CreatePriorityQueue()
    {
      List<HeapNode> listFreq = new List<HeapNode>();
      foreach (var pair in _characterFrequency)
      {
        listFreq.Add(new HeapNode(pair.Key, pair.Value));
      }

      priorityQueue.Heapify(listFreq);
    }

    public void BuildHuffmanTreeFromPriorityQueue()
    {
      while (priorityQueue.heap.Count > 2)
      {
        var left = priorityQueue.Pop();
        var leftNode = new HuffmanNode(left.Character, left.Frequency);
        var right = priorityQueue.Pop();
        var rightNode = new HuffmanNode(right.Character, right.Frequency);

        var newNode = new HuffmanNode(null, left.Frequency + right.Frequency);
        newNode.Left = leftNode;
        newNode.Right = rightNode;

        priorityQueue.Push(newNode.Character, newNode.Frequency);
        root = newNode;
      }
    }

    public void BuildTree()
    {
      BuildHuffmanTreeFromPriorityQueue();
    }
  }
}