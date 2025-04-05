using System.Collections.Generic;


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
  }

}