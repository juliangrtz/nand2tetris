namespace HackAssembler.Macros;

public class Macro : IMacro
{
    private readonly string _code;
    public Macro(string code)
    {
        _code = code;
    }

    public virtual ushort GetNumberOfArguments()
        => 0;

    public virtual string[] GetAssemblyCode()
        => throw new NotImplementedException();

    public List<string> GetArguments()
    {
        var arguments = _code.Split(" ").ToList();
        var macroName = arguments[0];
        arguments.RemoveAt(0); // Erstes Argument ist der Name des Makros

        if (arguments.Count != GetNumberOfArguments())
            throw new Exception($"Did not supply {GetNumberOfArguments()} arguments to the macro {macroName}!");

        return arguments;
    }
}
