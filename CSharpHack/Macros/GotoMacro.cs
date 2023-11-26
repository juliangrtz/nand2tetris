namespace HackAssembler.Macros;

public class GotoMacro : Macro
{
    public GotoMacro(string code) : base(code) { }

    public override ushort GetNumberOfArguments()
        => 1;

    public override string[] GetAssemblyCode()
    {
        var arguments = GetArguments();
        var address = arguments[0];

        return new[]
        {
            $"@{address}",
            "0;JMP"
        };
    }
}
