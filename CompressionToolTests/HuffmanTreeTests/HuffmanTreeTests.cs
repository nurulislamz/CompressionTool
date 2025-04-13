using CompressionTool;
using NUnit.Framework;

namespace CompressionToolTests;

public class HuffmanTreeTests
{
    private PriorityQueue _priorityQueue;
    private HuffmanTree _huffmanTree;

    [SetUp]
    public void Setup()
    {
        _priorityQueue = new PriorityQueue();
        _priorityQueue.Push('a', 3);
        _priorityQueue.Push('b', 1);
        _priorityQueue.Push('c', 2);
    }

    [Test]
    public void BuildHuffmanTreeFromPriorityQueue_CreatesCorrectTree()
    {
        // Act
        _huffmanTree = new HuffmanTree();
        _huffmanTree.BuildHuffmanTreeFromPriorityQueue(_priorityQueue);

        // Assert
        var root = _huffmanTree.root;
        Assert.That(root, Is.Not.Null);
        Assert.That(root?.Frequency, Is.EqualTo(6)); // Total frequency
        Assert.That(root?.Left?.Frequency, Is.EqualTo(3)); // 'a' node
        Assert.That(root?.Right?.Frequency, Is.EqualTo(3)); // Combined 'b' and 'c' node
    }

    [Test]
    public void HuffmanNode_Creation_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var node = new HuffmanNode('a', 3);

        // Assert
        Assert.That(node.Character, Is.EqualTo('a'));
        Assert.That(node.Frequency, Is.EqualTo(3));
        Assert.That(node.Left, Is.Null);
        Assert.That(node.Right, Is.Null);
    }

    [Test]
    public void HuffmanNode_WithChildren_SetsPropertiesCorrectly()
    {
        // Arrange
        var left = new HuffmanNode('a', 3);
        var right = new HuffmanNode('b', 2);

        // Act
        var parent = new HuffmanNode(null, 5)
        {
            Left = left,
            Right = right
        };

        // Assert
        Assert.That(parent.Character, Is.Null);
        Assert.That(parent.Frequency, Is.EqualTo(5));
        Assert.That(parent.Left, Is.EqualTo(left));
        Assert.That(parent.Right, Is.EqualTo(right));
    }
}
