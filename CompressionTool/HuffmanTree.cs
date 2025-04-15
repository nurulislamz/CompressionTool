using System.Text;
using System.IO;

namespace CompressionTool
{
  public class HuffmanTree
  {
    public HuffmanNode? root;
    private Dictionary<char, List<bool>> _encodingMap;

    public HuffmanTree()
    {
      _encodingMap = new Dictionary<char, List<bool>>();
    }

    public void BuildHuffmanTreeFromPriorityQueue(IPriorityQueue priorityQueue)
    {
      if (priorityQueue.Count() <= 1) // Check for dummy node
      {
        throw new InvalidOperationException("Cannot build Huffman tree from an empty or invalid priority queue");
      }

      while (priorityQueue.Count() > 2)
      {
        // Pop the two nodes with the lowest frequencies, skipping the dummy node
        var left = priorityQueue.Pop();
        var right = priorityQueue.Pop();

        // Create a new internal node with these two as children
        var newNode = new HuffmanNode(null, left.Frequency + right.Frequency)
        {
          Left = left,
          Right = right
        };

        // Push the new internal node back to the queue
        priorityQueue.Push(newNode);
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

    public Dictionary<char, List<bool>> GetEncodingMapAsDictionary()
    {
      return _encodingMap.ToDictionary(
          kvp => kvp.Key,
          kvp => new List<bool>(kvp.Value)
      );
    }

    public byte[] ConvertHuffmanTreeToByteArray()
    {
      using (var memoryStream = new MemoryStream())
      {
        WriteHuffmanTree(memoryStream, root);
        return memoryStream.ToArray();
      }
    }

    private void WriteHuffmanTree(MemoryStream memoryStream, HuffmanNode node)
    {
      if (node == null) return;

      using (var writer = new BinaryWriter(memoryStream, Encoding.UTF8, true))
      {
        if (node.Character.HasValue)
        {
          // Write a marker for leaf nodes
          writer.Write((byte)1); // Indicates a leaf node
          writer.Write((byte)node.Character.Value); // Write the character
          writer.Write(node.Frequency); // Write the frequency
        }
        else
        {
          // Write a marker for internal nodes
          writer.Write((byte)0); // Indicates an internal node
          WriteHuffmanTree(memoryStream, node.Left);
          WriteHuffmanTree(memoryStream, node.Right);
        }
      }
    }
  }
}