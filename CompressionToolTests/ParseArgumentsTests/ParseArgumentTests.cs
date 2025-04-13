using CompressionTool;
using System.IO;
using System.Reflection;

namespace CompressionToolTests;

public class ParseArgumentTests
{
    private string[] _testArgs;
    private string[] _testArgsWithInvalidArgs;

    private IFileOpener _fileOpener;
    private IFrequencyCounter _frequencyCounter;
    private string _testFilePath;

    [SetUp]
    public void Setup()
    {
        // Get the test file path relative to the test project
        var testProjectDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _testFilePath = Path.Combine(testProjectDir, "TestData", "test1.txt");

        // Create test file if it doesn't exist
        if (!File.Exists(_testFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_testFilePath));
            File.WriteAllText(_testFilePath, "abcdefghijklmnopqrstuvwxyz");
        }
        
        _testArgs = new string[]
        {
            "-f",
             _testFilePath,
            "-m",
            "Compress"
        };

        _testArgsWithInvalidArgs = new string[]
        {
            "-f",
            "DNE",
            "-m",
            "abcd"
        };
    }

    [Test]
    [TestCase(ModeOptions.Compress)]
    [TestCase(ModeOptions.Decompress)]
    public async Task ParseCommandLine_WithValidArgs_ReturnsCorrectResult(ModeOptions modeOptions)
    {
        var commandLineParser = new ArgumentParser();

        var actualResult = await commandLineParser.ParseCommandLine(_testArgs);
        var projectDir = Directory.GetCurrentDirectory();
        var expectedPath = Path.Combine(projectDir, "TestData", "test1.txt").Replace("\\", "/");
        var expectedResult = new ParsedResult
        {
            FilePath = expectedPath,
            Mode = ModeOptions.Compress
        };
        Assert.That(expectedResult.FilePath, Is.EqualTo(actualResult.FilePath));
        Assert.That(expectedResult.Mode, Is.EqualTo(actualResult.Mode));
    }

    [Test]
    public void ParseCommandLine_InvalidCompressArgs_ReturnsError()
    {
        var commandLineParser = new ArgumentParser();
        Assert.ThrowsAsync<ArgumentException>(() =>  commandLineParser.ParseCommandLine(_testArgsWithInvalidArgs));
    }
}