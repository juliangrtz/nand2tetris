namespace HackAssembler.Macros;

public interface IMacro
{
    ushort GetNumberOfArguments();

    string[] GetAssemblyCode();
}
