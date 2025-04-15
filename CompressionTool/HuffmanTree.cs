using System.Text;
using System.IO;
using System.Text.Json;

namespace CompressionTool
{
  public interface IHuffmanTree
  {
    void BuildHuffmanTreeFromPriorityQueue(IPriorityQueue priorityQueue);
    void EncodeHuffmanTree();
    Dictionary<char, List<bool>> GetEncodingMapAsDictionary();
    byte[] ConvertHuffmanTreeToByteArray();
    HuffmanTree ConvertByteArrayToHuffmanTree(byte[] bytes);
  }

  public class HuffmanTree : IHuffmanTree
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

    public Dictionary<char, List<bool>> GetEncodingMapAsDictionary() => _encodingMap;

    public byte[] ConvertHuffmanTreeToByteArray()
    {
      using (var memoryStream = new MemoryStream())
      using (var writer = new BinaryWriter(memoryStream, Encoding.UTF8, true))
      {
        WriteHuffmanTree(writer, root);
        return memoryStream.ToArray();
      }
    }

    private void WriteHuffmanTree(BinaryWriter writer, HuffmanNode node)
    {
      if (node == null) return;

      if (node.Character.HasValue)
      {
        writer.Write((byte)1); // Indicates a leaf node
        writer.Write((byte)node.Character.Value); // Write the character
        writer.Write(node.Frequency); // Write the frequency
      }
      else
      {
        writer.Write((byte)0); // Indicates an internal node
        WriteHuffmanTree(writer, node.Left);
        WriteHuffmanTree(writer, node.Right);
      }
    }

    public HuffmanTree ConvertByteArrayToHuffmanTree(byte[] bytes)
    {
      var tree = new HuffmanTree();
      using (var memoryStream = new MemoryStream(bytes))
      using (var reader = new BinaryReader(memoryStream, Encoding.UTF8, true))
      {
        tree.root = ReadHuffmanTreeNode(reader);
      }
      return tree;
    }

    private HuffmanNode ReadHuffmanTreeNode(BinaryReader reader)
    {
      // Read the node type marker
      byte nodeType = reader.ReadByte();

      if (nodeType == 1) // Leaf node
      {
        // Read the character and frequency
        char character = (char)reader.ReadByte();
        int frequency = reader.ReadInt32();
        return new HuffmanNode(character, frequency);
      }
      else // Internal node
      {
        // Create an internal node and recursively read its children
        var left = ReadHuffmanTreeNode(reader);
        var right = ReadHuffmanTreeNode(reader);

        var node = new HuffmanNode(null, left.Frequency + right.Frequency)
        {
          Left = left,
          Right = right
        };
        return node;
      }
    }

    public static string SaveFrequencyTableToJson(Dictionary<char, int> frequencyTable)
    {
      var jsonString = JsonSerializer.Serialize(frequencyTable);
      return jsonString;
    }
  }
}