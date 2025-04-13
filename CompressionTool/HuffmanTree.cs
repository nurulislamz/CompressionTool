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

    public bool IsLeah => Character.HasValue;
  }

  public class HuffmanTree
  {
    public HuffmanNode? root;
    private Dictionary<char, List<bool>> _encodingMap;

    public HuffmanTree()
    {
      _encodingMap = new Dictionary<char, List<bool>>();
    }

    public void BuildHuffmanTreeFromPriorityQueue(PriorityQueue priorityQueue)
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

    public void EncodeHuffmanTree()
    {
      if (root == null)
      {
        throw new Exception("HuffmanTree is empty");
      }

      GenerateEncodingsRecursive(root, new List<bool>());
    }

    private void GenerateEncodingsRecursive(HuffmanNode node, List<bool> encoding)
    {
        if (node.Character.HasValue)
        {
            _encodingMap[node.Character.Value] = new List<bool>(encoding);
            return;
        }

        if (node.Left != null)
        {
            encoding.Add(false); // 0
            GenerateEncodingsRecursive(node.Left, encoding);
            encoding.RemoveAt(encoding.Count - 1);
        }
        if (node.Right != null)
        {
            encoding.Add(true); // 1
            GenerateEncodingsRecursive(node.Right, encoding);
            encoding.RemoveAt(encoding.Count - 1);
        }
    }

  }
}