namespace HackAssembler.Instructions;

/// <summary>
/// Allgemeine Repräsentation einer Hack-Instruktion.
/// </summary>
public interface IInstruction
{
    /// <summary>
    /// Gibt eine binäre Darstellung der Instruktion zurück.
    /// </summary>
    string GetBinaryCode();
}

