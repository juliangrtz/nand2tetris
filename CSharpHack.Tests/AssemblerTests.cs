namespace HackAssembler.Tests;

public class AssemblerTests
{
    private Preprocessor _preprocessor = null!;

    [SetUp]
    public void Setup()
    {
        _preprocessor = new Preprocessor();
    }

    [Test]
    public void TestSamplePrograms()
    {
        foreach (var file in Directory.GetFiles("Samples", "*.asm"))
        {
            var content = File.ReadAllLines(file);
            var actualResult = new Assembler(_preprocessor.Preprocess(content)).Assemble();
            var expectedResult = File.ReadAllLines(file.Replace(".asm", ".hack"));

            Assert.That(expectedResult, Is.EqualTo(actualResult));
        }
    }
}
