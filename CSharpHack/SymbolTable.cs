namespace HackAssembler;

/// <summary>
/// Die Symboltabelle des Hack-Assemblers gemäß der Spezifikation.
/// </summary>
public class SymbolTable
{
    private readonly Dictionary<string, ushort> _symbolTable = new();

    /// <summary>
    /// Initialisiert die Symboltabelle mit vordefinierten Symbolen.
    /// </summary>
    public SymbolTable()
    {
        // Register R0 bis R15
        for (ushort r = 0; r < 16; r++)
            _symbolTable.Add($"R{r}", r);

        // I/O
        _symbolTable.Add("SCREEN", 16384);
        _symbolTable.Add("KBD", 24576);

        // Andere Symbole für die VM
        _symbolTable.Add("SP", 0);
        _symbolTable.Add("LCL", 1);
        _symbolTable.Add("ARG", 2);
        _symbolTable.Add("THIS", 3);
        _symbolTable.Add("THAT", 4);
    }

    /**
     * CRUD-Methoden
     */

    public bool Contains(string symbol)
        => _symbolTable.ContainsKey(symbol);

    public ushort GetAddress(string symbol)
        => _symbolTable[symbol];

    public void AddSymbol(string symbol, ushort address)
    {
        if(!Contains(symbol))
            _symbolTable.Add(symbol, address);
    }

    public void UpdateSymbol(string symbol, ushort address)
        => _symbolTable[symbol] = address;

    public void RemoveSymbol(string symbol)
        => _symbolTable.Remove(symbol);
}
