namespace HackAssembler.Instructions;

/// <summary>
/// Eine C-Instruktion (compute instruction) im Format "dest = comp ; jump"
/// </summary>
public class CInstruction : IInstruction
{
    // Lookup-Tabellen
    private readonly Dictionary<string, string> _compDictionary = new();
    private readonly Dictionary<string, string> _destDictionary = new();
    private readonly Dictionary<string, string> _jumpDictionary = new();

    private readonly string _comp, _dest, _jump;

    public CInstruction(string comp, string dest, string jump = "")
    {
        _comp = comp;
        _dest = dest;
        _jump = jump;

       PopulateDictionaries();
    }

    private void PopulateDictionaries()
    {
        // Comp
        _compDictionary.Add("0", "0101010");
        _compDictionary.Add("1", "0111111");
        _compDictionary.Add("-1", "0111010");
        _compDictionary.Add("D", "0001100");
        _compDictionary.Add("A", "0110000");
        _compDictionary.Add("M", "1110000");
        _compDictionary.Add("!D", "0001101");
        _compDictionary.Add("!A", "0110001");
        _compDictionary.Add("!M", "1110001");
        _compDictionary.Add("-D", "0001111");
        _compDictionary.Add("-A", "0110011");
        _compDictionary.Add("-M", "1110011");
        _compDictionary.Add("D+1", "0011111");
        _compDictionary.Add("A+1", "0110111");
        _compDictionary.Add("M+1", "1110111");
        _compDictionary.Add("D-1", "0001110");
        _compDictionary.Add("A-1", "0110010");
        _compDictionary.Add("M-1", "1110010");
        _compDictionary.Add("D+A", "0000010");
        _compDictionary.Add("D+M", "1000010");
        _compDictionary.Add("D-A", "0010011");
        _compDictionary.Add("D-M", "1010011");
        _compDictionary.Add("A-D", "0000111");
        _compDictionary.Add("M-D", "1000111");
        _compDictionary.Add("D&A", "0000000");
        _compDictionary.Add("D&M", "1000000");
        _compDictionary.Add("D|A", "0010101");
        _compDictionary.Add("D|M", "1010101");

        // Dest
        _destDictionary.Add("", "000");
        _destDictionary.Add("M", "001");
        _destDictionary.Add("D", "010");
        _destDictionary.Add("MD", "011");
        _destDictionary.Add("A", "100");
        _destDictionary.Add("AM", "101");
        _destDictionary.Add("AD", "110");
        _destDictionary.Add("AMD", "111");

        // Jump
        _jumpDictionary.Add("", "000");
        _jumpDictionary.Add("JGT", "001");
        _jumpDictionary.Add("JEQ", "010");
        _jumpDictionary.Add("JGE", "011");
        _jumpDictionary.Add("JLT", "100");
        _jumpDictionary.Add("JNE", "101");
        _jumpDictionary.Add("JLE", "110");
        _jumpDictionary.Add("JMP", "111");
    }

    private string GetCompBits()
        => _compDictionary[_comp];

    private string GetDestBits()
        => _destDictionary[_dest];

    private string GetJumpBits()
        => _jumpDictionary[_jump];

    // 1 1 1 a c1 c2 c3 c4 c5 c6 d1 d2 d3 j1 j2 j3
    /// <inheritdoc cref="IInstruction"/>
    public string GetBinaryCode()
        => "111" + GetCompBits() + GetDestBits() + GetJumpBits(); // C-Instruktionen beginnen immer mit "111".
}
