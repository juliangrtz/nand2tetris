namespace HackAssembler.Tests;

public class SymbolTableTests
{
    private SymbolTable _symbolTable = null!;

    [SetUp]
    public void Setup()
    {
        _symbolTable = new SymbolTable();
    }

    [Test]
    public void TestRegisters()
    {
        for (ushort r = 0; r < 16; r++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(_symbolTable.Contains($"R{r}"), Is.True);
                Assert.That(r, Is.EqualTo(_symbolTable.GetAddress($"R{r}")));
            });
        }
    }

    [Test]
    public void TestIO()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_symbolTable.Contains("SCREEN"), Is.True);
            Assert.That(_symbolTable.GetAddress("SCREEN"), Is.EqualTo(16384));

            Assert.That(_symbolTable.Contains("KBD"), Is.True);
            Assert.That(_symbolTable.GetAddress("KBD"), Is.EqualTo(24576));
        });
    }

    [Test]
    public void TestVMSymbols()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_symbolTable.Contains("SP"), Is.True);
            Assert.That(_symbolTable.GetAddress("SP"), Is.EqualTo(0));

            Assert.That(_symbolTable.Contains("LCL"), Is.True);
            Assert.That(_symbolTable.GetAddress("LCL"), Is.EqualTo(1));

            Assert.That(_symbolTable.Contains("ARG"), Is.True);
            Assert.That(_symbolTable.GetAddress("ARG"), Is.EqualTo(2));

            Assert.That(_symbolTable.Contains("THIS"), Is.True);
            Assert.That(_symbolTable.GetAddress("THIS"), Is.EqualTo(3));

            Assert.That(_symbolTable.Contains("THAT"), Is.True);
            Assert.That(_symbolTable.GetAddress("THAT"), Is.EqualTo(4));
        });
    }

    [Test]
    public void TestCRUDMethods()
    {
        _symbolTable.AddSymbol("example", 42);
        Assert.Multiple(() =>
        {
            Assert.That(_symbolTable.Contains("example"));
            Assert.That(_symbolTable.GetAddress("example"), Is.EqualTo(42));
        });

        _symbolTable.UpdateSymbol("example", 43);
        Assert.That(_symbolTable.GetAddress("example"), Is.EqualTo(43));

        _symbolTable.RemoveSymbol("example");
        Assert.That(!_symbolTable.Contains("example"));
    }
}
