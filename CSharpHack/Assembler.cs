using HackAssembler.Instructions;

namespace HackAssembler;

/// <summary>
/// Diese Klasse wandelt ein Assembly-Programm in ein Hack-Programm um.
/// </summary>
public class Assembler
{
    private readonly string[] _preprocessedHackProgram;
    private readonly Parser _parser;
    private readonly SymbolTable _symbolTable;

    private ushort _programCounter;
    private ushort _variableAddressCounter = 16;

    public Assembler(string[] preprocessedHackProgram)
    {
        _preprocessedHackProgram = preprocessedHackProgram;
        _parser = new Parser();
        _symbolTable = new SymbolTable();
    }

    private string ConvertAInstruction(string instruction)
    {
        var symbol = instruction.Replace("@", "");

        if (ushort.TryParse(symbol, out var address))
        {
            // Adresse ist hartkodiert als Zahl
            return new AInstruction(address).GetBinaryCode();
        }
        else if (_symbolTable.Contains(symbol))
        {
            // Bekanntes Symbol in der Symboltabelle
            // => Symbol zur Symboltabelle hinzufügen
            var symbolTableAddress = _symbolTable.GetAddress(symbol);
            return new AInstruction(symbolTableAddress).GetBinaryCode();
        }
        else
        {
            // Unbekanntes Symbol in der Symboltabelle
            // => Symbol zur Symboltabelle hinzufügen
            _symbolTable.AddSymbol(symbol, _variableAddressCounter);
            var result = new AInstruction(_variableAddressCounter).GetBinaryCode();
            _variableAddressCounter++;
            return result;
        }
    }

    private string ConvertCInstruction(string instruction)
    {
        var comp = _parser.GetCompPart(instruction);
        var dest = _parser.GetDestPart(instruction);
        var jump = _parser.GetJumpPart(instruction);

        return new CInstruction(comp, dest, jump).GetBinaryCode();
    }

    public string[] Assemble()
    {
        // Erster Durchlauf: Labels zur Symboltabelle hinzufügen
        foreach (var instruction in _preprocessedHackProgram)
        {
            var type = _parser.GetInstructionType(instruction);

            if (type == Parser.InstructionType.LInstruction)
            {
                var symbol = instruction.Replace("(", "").Replace(")", "");
                _symbolTable.AddSymbol(symbol, _programCounter--);
            }

            _programCounter++;
        }

        // Zweiter Durchlauf: A- und C-Instruktionen parsen
        var result = new List<string>();

        foreach (var instruction in _preprocessedHackProgram)
        {
            var type = _parser.GetInstructionType(instruction);

            if (type == Parser.InstructionType.AInstruction)
                result.Add(ConvertAInstruction(instruction));
            else if (type == Parser.InstructionType.CInstruction)
                result.Add(ConvertCInstruction(instruction));
        }

        return result.ToArray();
    }
}
