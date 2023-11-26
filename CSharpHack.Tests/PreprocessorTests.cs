namespace HackAssembler.Tests;

public class PreprocessorTests
{
    private Preprocessor _preprocessor = null!;

    [SetUp]
    public void Setup()
    {
        _preprocessor = new Preprocessor();
    }

    [Test]
    public void TestBlankLines()
    {
        var blankLines = @"
            @42

             M=1
                    @R0  
                AM=M+1
        ".Split("\n");

        var actualResult = _preprocessor.Preprocess(blankLines);
        var expectedResult = "@42\nM=1\n@R0\nAM=M+1".Split("\n");

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void TestComments()
    {
        var comments = @"
            // Hier ein Kommentar.
            //// Hier ein weiterer Kommentar.
            
            @42
             M=1 // ...und hier ein Kommentar hinter einer Zeile!
                    @R0  
                AM=M+1
        ".Split("\n");

        var actualResult = _preprocessor.Preprocess(comments);
        var expectedResult = "@42\nM=1\n@R0\nAM=M+1".Split("\n");

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
}
