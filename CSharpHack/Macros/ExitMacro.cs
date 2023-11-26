namespace HackAssembler.Macros;

internal class ExitMacro : Macro
{
    public ExitMacro(string code) : base(code) { }

    public override ushort GetNumberOfArguments()
        => 0;

    public override string[] GetAssemblyCode()
    {
        return new[]
        {
                "(EXIT)",
                "@EXIT",
                "0;JMP"
            };
    }
}
