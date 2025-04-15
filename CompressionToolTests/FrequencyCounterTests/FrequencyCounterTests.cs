using CompressionTool;
using System.IO;
using System.Reflection;

namespace CompressionToolTests;

public class FrequencyCounterTests
{
    private IFrequencyCounter _frequencyCounter;
    private string _testFilePath;

    [SetUp]
    public void Setup()
    {
        _frequencyCounter = new FrequencyCounter();

        // Get the test file path relative to the test project
        var testProjectDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _testFilePath = Path.Combine(testProjectDir, "TestData", "test1.txt");

        // Create test file if it doesn't exist
        if (!File.Exists(_testFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_testFilePath));
            File.WriteAllText(_testFilePath, "abcdefghijklmnopqrstuvwxyz");
        }
    }

    [Test]
    public void FrequencyCounter_ReturnsCorrectResult()
    {
        string simpleString = "aaabbbcccddd";
        var actualResult = _frequencyCounter.CountFrequency(simpleString);
        var expectedResult = new Dictionary<char, int>()
        {
            { 'a', 3 },
            { 'b', 3 },
            { 'c', 3 },
            { 'd', 3 },
        };

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void FrequencyCounter_WithTestFile_ReturnsCorrectResult()
    {
        // Arrange
        var fileContent = File.ReadAllText(_testFilePath);

        // Act
        var actualResult = _frequencyCounter.CountFrequency(fileContent);

        // Assert
        var expectedResult = new Dictionary<char, int>()
        {
            { 'a', 1 },
            { 'b', 1 },
            { 'c', 1 },
            { 'd', 1 },
            { 'e', 1 },
            { 'f', 1 },
            { 'g', 1 },
            { 'h', 1 },
            { 'i', 1 },
            { 'j', 1 },
            { 'k', 1 },
            { 'l', 1 },
            { 'm', 1 },
            { 'n', 1 },
            { 'o', 1 },
            { 'p', 1 },
            { 'q', 1 },
            { 'r', 1 },
            { 's', 1 },
            { 't', 1 },
            { 'u', 1 },
            { 'v', 1 },
            { 'w', 1 },
            { 'x', 1 },
            { 'y', 1 },
            { 'z', 1 }
        };

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up test file if needed
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }
}