using System.Text;
using System.IO;

namespace CompressionTool
{
  public class CompressedData
  {
    public int OriginalFileSize { get; set; }
    public int TreeSize { get; set; }
    public byte[] TreeData { get; set; }
    public byte[] CompressedContent { get; set; }
  }

  public interface ICompress
  {
    CompressedData CompressText(string text);
    void SaveCompressedFile(string inputText, string outputPath);
    string DecompressFile(string inputPath);
  }

  public class Compress : ICompress
  {
    private readonly IFrequencyCounter _frequencyCounter;
    private readonly IHuffmanTree _huffmanTree;
    private readonly IPriorityQueue _priorityQueue;

    public Compress(
        IFrequencyCounter frequencyCounter,
        IHuffmanTree huffmanTree,
        IPriorityQueue priorityQueue)
    {
      _frequencyCounter = frequencyCounter ?? throw new ArgumentNullException(nameof(frequencyCounter));
      _huffmanTree = huffmanTree ?? throw new ArgumentNullException(nameof(huffmanTree));
      _priorityQueue = priorityQueue ?? throw new ArgumentNullException(nameof(priorityQueue));
    }

    public CompressedData CompressText(string text)
    {
      // Create frequency table and build tree first
      var frequencies = _frequencyCounter.CountFrequency(text);
      foreach (var kvp in frequencies)
      {
        _priorityQueue.Push(new HuffmanNode(kvp.Key, kvp.Value));
      }
      _huffmanTree.BuildHuffmanTreeFromPriorityQueue(_priorityQueue);
      _huffmanTree.EncodeHuffmanTree();

      var treeData = _huffmanTree.ConvertHuffmanTreeToByteArray();
      // Prepare the compressed data structure
      var compressedData = new CompressedData
      {
        OriginalFileSize = text.Length,
        TreeData = treeData,
        TreeSize = treeData.Length,
        CompressedContent = CompressContent(text)
      };

      return compressedData;
    }

    private byte[] CompressContent(string text)
    {
      // Create a bit array to store all the bits
      var bits = new List<bool>();
      var encodingMap = _huffmanTree.GetEncodingMapAsDictionary();

      // Convert each character to its Huffman code
      foreach (char c in text)
      {
        if (encodingMap.ContainsKey(c))
        {
          bits.AddRange(encodingMap[c]);
        }
      }

      // Convert bits to bytes
      return ConvertBitsToBytes(bits);
    }

    private byte[] ConvertBitsToBytes(List<bool> bits)
    {
      // Calculate how many bytes we need
      int numBytes = (bits.Count + 7) / 8;
      byte[] bytes = new byte[numBytes];

      // Convert each group of 8 bits to a byte
      for (int i = 0; i < bits.Count; i += 8)
      {
        byte b = 0;
        for (int j = 0; j < 8 && i + j < bits.Count; j++)
        {
          if (bits[i + j])
          {
            b |= (byte)(1 << (7 - j));
          }
        }
        bytes[i / 8] = b;
      }

      return bytes;
    }

    public void SaveCompressedFile(string inputText, string outputPath)
    {
      var compressedData = CompressText(inputText);

      using (var fileStream = new FileStream(outputPath, FileMode.Create))
      using (var writer = new BinaryWriter(fileStream))
      {
        // Write header information
        writer.Write(compressedData.OriginalFileSize);
        writer.Write(compressedData.TreeSize);

        // Write tree data
        writer.Write(compressedData.TreeData);

        // Write compressed content
        writer.Write(compressedData.CompressedContent);
      }
    }

    public string DecompressFile(string inputPath)
    {
      using (var fileStream = new FileStream(inputPath, FileMode.Open))
      using (var reader = new BinaryReader(fileStream))
      {
        // Read header information
        int originalSize = reader.ReadInt32();
        int treeSize = reader.ReadInt32();

        // Read and reconstruct the tree
        byte[] treeData = reader.ReadBytes(treeSize);
        var huffmanTree = _huffmanTree.ConvertByteArrayToHuffmanTree(treeData);

        // Read compressed content
        byte[] compressedContent = reader.ReadBytes((int)(fileStream.Length - fileStream.Position));

        // Decompress the content
        return DecompressContent(compressedContent, originalSize, treeData);
      }
    }

    private string DecompressContent(byte[] compressedBytes, int originalSize, byte[] treeData)
    {
      var result = new StringBuilder();
      var tree = _huffmanTree.ConvertByteArrayToHuffmanTree(treeData);
      var currentNode = tree.root;
      int bitIndex = 0;

      // Process each bit in the compressed data
      for (int i = 0; i < originalSize; i++)
      {
        while (currentNode.Character == null)
        {
          bool bit = GetBit(compressedBytes, bitIndex++);
          currentNode = bit ? currentNode.Right : currentNode.Left;
        }

        result.Append(currentNode.Character.Value);
        currentNode = tree.root;
      }

      return result.ToString();
    }

    private bool GetBit(byte[] bytes, int bitIndex)
    {
      int byteIndex = bitIndex / 8;
      int bitPosition = 7 - (bitIndex % 8);
      return (bytes[byteIndex] & (1 << bitPosition)) != 0;
    }
  }
}