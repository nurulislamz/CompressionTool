using System.Collections.Generic;

public class Node
{
  public char Character { get; set; }
  public int Frequency { get; set; }
  public Node? Left { get; set; }
  public Node? Right { get; set; }

  public Node(char character, int frequency)
  {
    Character = character;
    Frequency = frequency;
    Left = null;
    Right = null;
  }
}

public class HuffmanTree
{
  private Node? _root;
  private Dictionary<char, int> _codes;

  public HuffmanTree(Dictionary<char, int> characterFrequency)
  {
    _codes = new Dictionary<char, int>();
  }

  public void BuildTree()
  {
    var priorityQueue = new PriorityQueue<Node, int>();

    foreach (var character in _codes)
    {
      priorityQueue.Enqueue(new Node(character.Key, character.Value), character.Value);
    }

    while (priorityQueue.Count > 1)
    {
      var left = priorityQueue.Dequeue();
      var right = priorityQueue.Dequeue();
    }
  }

}