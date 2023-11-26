using HackAssembler.Instructions;

namespace HackAssembler.Tests.Instructions;

public class CInstructionTests
{
    [Test]
    public void TestBinaryConversion()
    {
        // dest = comp ; jump

        // M = 1
        var test1 = new CInstruction(comp:"1", dest:"M", jump: "");
        Assert.That(test1.GetBinaryCode(), Is.EqualTo("111" + "0111111" + "001" + "000"));

        // D; JLE
        var test2 = new CInstruction(comp: "D", dest: "", jump: "JLE");
        Assert.That(test2.GetBinaryCode(), Is.EqualTo("111" + "0001100" + "000" + "110"));

        // AM = M+1
        var test3 = new CInstruction(comp: "M+1", dest: "AM", jump: "");
        Assert.That(test3.GetBinaryCode(), Is.EqualTo("111" + "1110111" + "101" + "000"));
    }
}
