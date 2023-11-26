namespace HackAssembler.Instructions;

/// <summary>
/// Eine A-Instruktion (address instruction) in Hack-Assembly.
/// </summary>
public class AInstruction : IInstruction
{
    private readonly ushort _address;

    public AInstruction(ushort address)
    {
        _address = address;
    }

    private string To15BitNumber()
        => Convert.ToString(_address, 2).PadLeft(15, '0');

    /// <summary>
    /// Prüft den Wertebereich der übergebenen Adresse.
    /// </summary>
    /// <returns>0 &lt;= address &lt;= 32767</returns>
    private bool CheckDomain()
        => _address < Math.Pow(2, 15);

    /// <inheritdoc cref="IInstruction"/>
    public string GetBinaryCode()
    {
        if (CheckDomain())
            return "0" + To15BitNumber(); // Das erste Bit "0" gibt den Instruktionstyp an.

        throw new InvalidOperationException("The address cannot be bigger than 2^15 - 1.");
    }
}
