using CompressionTool;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompressionToolTests;

public class HuffmanTreeTests
{
    private PriorityQueue _priorityQueue;
    private HuffmanTree _huffmanTree;

    [SetUp]
    public void Setup()
    {
        _priorityQueue = new PriorityQueue();
        _priorityQueue.Push(new HuffmanNode('a', 3));
        _priorityQueue.Push(new HuffmanNode('b', 1));
        _priorityQueue.Push(new HuffmanNode('c', 2));
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

        // Verify 'b' and 'c' nodes exist in the tree
        var rightNode = root?.Right;
        Assert.That(rightNode, Is.Not.Null, "Right node should exist");
        Assert.That(rightNode?.Left?.Character, Is.EqualTo('b'), "Left child of right node should be 'b'");
        Assert.That(rightNode?.Right?.Character, Is.EqualTo('c'), "Right child of right node should be 'c'");
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

    [Test]
    public void EncodeHuffmanTree_GeneratesCorrectEncodings()
    {
        // Arrange
        _huffmanTree = new HuffmanTree();
        _huffmanTree.BuildHuffmanTreeFromPriorityQueue(_priorityQueue);

        // Act
        _huffmanTree.EncodeHuffmanTree();

        // Get encodings through a new method we'll add
        var encodings = _huffmanTree.GetEncodingMapAsDictionary();

        // Assert
        // For the tree with frequencies: a=3, b=1, c=2
        // The tree should look like:
        //       (6)
        //      /   \
        //    (3)   (3)
        //   /        \
        //  a       (b,c)
        //          /   \
        //         b     c
        Assert.That(encodings['a'], Is.EqualTo(new List<bool> { false }));     // 'a' should be '0'
        Assert.That(encodings['b'], Is.EqualTo(new List<bool> { true, false })); // 'b' should be '10'
        Assert.That(encodings['c'], Is.EqualTo(new List<bool> { true, true }));  // 'c' should be '11'
    }

    [Test]
    public void EncodeHuffmanTree_WithEmptyTree_ThrowsException()
    {
        // Arrange
        _huffmanTree = new HuffmanTree();

        // Act & Assert
        Assert.Throws<Exception>(() => _huffmanTree.EncodeHuffmanTree());
    }

    [Test]
    public void EncodeHuffmanTree_EncodingsAreUnique()
    {
        // Arrange
        _huffmanTree = new HuffmanTree();
        _huffmanTree.BuildHuffmanTreeFromPriorityQueue(_priorityQueue);

        // Act
        _huffmanTree.EncodeHuffmanTree();
        var encodings = _huffmanTree.GetEncodingMapAsDictionary();

        // Assert
        var uniqueEncodings = new HashSet<string>(
            encodings.Values.Select(bits => string.Join("", bits.Select(b => b ? "1" : "0")))
        );
        Assert.That(uniqueEncodings.Count, Is.EqualTo(encodings.Count), "Each character should have a unique encoding");
    }

    [Test]
    public void EncodeHuffmanTree_PrefixProperty()
    {
        // Arrange
        _huffmanTree = new HuffmanTree();
        _huffmanTree.BuildHuffmanTreeFromPriorityQueue(_priorityQueue);

        // Act
        _huffmanTree.EncodeHuffmanTree();
        var encodings = _huffmanTree.GetEncodingMapAsDictionary();

        // Assert
        // Check that no encoding is a prefix of another encoding
        foreach (var encoding1 in encodings.Values)
        {
            foreach (var encoding2 in encodings.Values)
            {
                if (!encoding1.SequenceEqual(encoding2))
                {
                    var shorterLength = Math.Min(encoding1.Count, encoding2.Count);
                    var isPrefix = encoding1.Take(shorterLength).SequenceEqual(encoding2.Take(shorterLength));
                    Assert.That(isPrefix, Is.False, "No encoding should be a prefix of another encoding");
                }
            }
        }
    }

    [Test]
    public void GetEncodingMapAsDictionary_And_GetEncodingMapAsString_ProduceConsistentResults()
    {
        // Arrange
        _huffmanTree = new HuffmanTree();
        _huffmanTree.BuildHuffmanTreeFromPriorityQueue(_priorityQueue);
        _huffmanTree.EncodeHuffmanTree();

        // Act
        var encodingDict = _huffmanTree.GetEncodingMapAsDictionary();
        var encodingString = _huffmanTree.();

        // Assert
        // Convert dictionary to string format for comparison
        var dictAsString = string.Join("\n", encodingDict.Select(kvp =>
            $"{kvp.Key}:{string.Join("", kvp.Value.Select(b => b ? "1" : "0"))}"
        ));

        // Compare the string representations
        Assert.That(encodingString, Is.EqualTo(dictAsString),
            "String representation should match dictionary representation");
    }
}
