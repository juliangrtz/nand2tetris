using HackAssembler.Instructions;

namespace HackAssembler.Tests.Instructions;

public class AInstructionTests
{
    [Test]
    public void TestBinaryConversion()
    {
        var aIns = new AInstruction(0);
        Assert.That(aIns.GetBinaryCode(), Is.EqualTo(string.Concat(Enumerable.Repeat("0", 16))));

        aIns = new AInstruction(42);
        Assert.That(aIns.GetBinaryCode(), Is.EqualTo("0000000000101010"));

        aIns = new AInstruction(32767);
        Assert.That(aIns.GetBinaryCode(), Is.EqualTo("0111111111111111"));

        aIns = new AInstruction(32768);
        Assert.Throws<InvalidOperationException>(() => aIns.GetBinaryCode());
    }
}
