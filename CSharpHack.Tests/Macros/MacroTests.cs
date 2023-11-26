namespace HackAssembler.Tests.Macros;

public class MacroTests
{
    private Preprocessor _preprocessor = null!;

    [SetUp]
    public void Setup()
    {
        _preprocessor = new Preprocessor();
    }

    [Test]
    public void TestGotoMacro()
    {
        var gotoMacro = new[] { "goto 42" };
        var expectedAssembly = "@42\n0;JMP".Split("\n");

        Assert.That(_preprocessor.Preprocess(gotoMacro), Is.EqualTo(expectedAssembly));

        gotoMacro = new[] { "goto SCREEN" };
        expectedAssembly = "@SCREEN\n0;JMP".Split("\n");

        Assert.That(_preprocessor.Preprocess(gotoMacro), Is.EqualTo(expectedAssembly));

        // Fehlendes Argument
        gotoMacro = new[] { "goto" };
        Assert.Throws<Exception>(() => _preprocessor.Preprocess(gotoMacro));
    }

    [Test]
    public void TestExitMacro()
    {
        var gotoMacro = new[] { "@42", "exit" };
        var expectedAssembly = "@42\n(EXIT)\n@EXIT\n0;JMP".Split("\n");

        Assert.That(_preprocessor.Preprocess(gotoMacro), Is.EqualTo(expectedAssembly));
    }
}
