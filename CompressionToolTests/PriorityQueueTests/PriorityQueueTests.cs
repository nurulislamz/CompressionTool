using CompressionTool;
using NUnit.Framework;

namespace CompressionToolTests;

public class PriorityQueueTests
{
    private PriorityQueue _priorityQueue;

    [SetUp]
    public void Setup()
    {
        _priorityQueue = new PriorityQueue();
    }

    [Test]
    public void Push_AddsNode_InCorrectOrder()
    {
        // Arrange & Act
        _priorityQueue.Push(new HuffmanNode('a', 3));
        _priorityQueue.Push(new HuffmanNode('b', 1));
        _priorityQueue.Push(new HuffmanNode('c', 2));

        // Assert
        var top = _priorityQueue.Top();
        Assert.That(top?.Frequency, Is.EqualTo(1));
        Assert.That(top?.Character, Is.EqualTo('b'));
    }

    [Test]
    public void Pop_RemovesNodes_InCorrectOrder()
    {
        // Arrange
        _priorityQueue.Push(new HuffmanNode('a', 3));
        _priorityQueue.Push(new HuffmanNode('b', 1));
        _priorityQueue.Push(new HuffmanNode('c', 2));

        // Act & Assert
        var first = _priorityQueue.Pop();
        Assert.That(first?.Frequency, Is.EqualTo(1));
        Assert.That(first?.Character, Is.EqualTo('b'));

        var second = _priorityQueue.Pop();
        Assert.That(second?.Frequency, Is.EqualTo(2));
        Assert.That(second?.Character, Is.EqualTo('c'));

        var third = _priorityQueue.Pop();
        Assert.That(third?.Frequency, Is.EqualTo(3));
        Assert.That(third?.Character, Is.EqualTo('a'));
    }

    [Test]
    public void Top_ReturnsFirstElement_WithoutRemoving()
    {
        // Arrange
        _priorityQueue.Push(new HuffmanNode('a', 1));

        // Act
        var peeked = _priorityQueue.Top();
        var popped = _priorityQueue.Pop();

        // Assert
        Assert.That(peeked?.Frequency, Is.EqualTo(1));
        Assert.That(peeked?.Character, Is.EqualTo('a'));
        Assert.That(popped?.Frequency, Is.EqualTo(1));
        Assert.That(popped?.Character, Is.EqualTo('a'));
    }

    [Test]
    public void Pop_EmptyQueue_ReturnsNull()
    {
        // Act & Assert
        Assert.Throws<Exception>(() => _priorityQueue.Pop());
    }

    [Test]
    public void Top_EmptyQueue_ReturnsNull()
    {
        // Act & Assert
        Assert.That(_priorityQueue.Top(), Is.Null);
    }
}
