namespace HackAssembler.Tests;

public class ParserTests
{
    private Parser _parser = null!;

    [SetUp]
    public void Setup()
    {
        _parser = new Parser();
    }

    [Test]
    public void TestGetInstructionType()
    {
        const string aIns1 = "@42";
        const string aIns2 = "@SCREEN";
        const string aIns3 = "@i";

        const string cIns1 = "0; JMP";
        const string cIns2 = "M=M+1";
        const string cIns3 = "AMD = M - 1";

        const string lIns = "(test)";

        Assert.Multiple(() =>
        {
            Assert.That(_parser.GetInstructionType(aIns1), Is.EqualTo(Parser.InstructionType.AInstruction));
            Assert.That(_parser.GetInstructionType(aIns2), Is.EqualTo(Parser.InstructionType.AInstruction));
            Assert.That(_parser.GetInstructionType(aIns3), Is.EqualTo(Parser.InstructionType.AInstruction));

            Assert.That(_parser.GetInstructionType(cIns1), Is.EqualTo(Parser.InstructionType.CInstruction));
            Assert.That(_parser.GetInstructionType(cIns2), Is.EqualTo(Parser.InstructionType.CInstruction));
            Assert.That(_parser.GetInstructionType(cIns3), Is.EqualTo(Parser.InstructionType.CInstruction));

            Assert.That(_parser.GetInstructionType(lIns), Is.EqualTo(Parser.InstructionType.LInstruction));
        });
    }

    [Test]
    public void TestGetCompPart()
    {
        const string test1 = "0;JMP";
        const string test2 = "M=M+1";

        Assert.Multiple(() =>
        {
            Assert.That(_parser.GetCompPart(test1), Is.EqualTo("0"));
            Assert.That(_parser.GetCompPart(test2), Is.EqualTo("M+1"));
        });
    }

    [Test]
    public void TestGetDestPart()
    {
        const string test1 = "M=M+1";
        const string test2 = "0;JMP";

        Assert.Multiple(() =>
        {
            Assert.That(_parser.GetDestPart(test1), Is.EqualTo("M"));
            Assert.That(_parser.GetDestPart(test2), Is.EqualTo(""));
        });
    }

    [Test]
    public void TestGetJumpPart()
    {
        const string test1 = "0;JMP";
        const string test2 = "D;JNZ";

        Assert.Multiple(() =>
        {
            Assert.That(_parser.GetJumpPart(test1), Is.EqualTo("JMP"));
            Assert.That(_parser.GetJumpPart(test2), Is.EqualTo("JNZ"));
        });
    }
}
