namespace HackAssembler;

/// <summary>
/// Ein Parser für eine einzelne Hack-Instruktion.
/// </summary>
public class Parser
{
    /// <summary>
    /// Die drei Instruktionstypen in der Hack-Assemblysprache.
    /// </summary>
    public enum InstructionType
    {
        AInstruction, // z. B. @42
        CInstruction, // z. B. AM=D+1
        LInstruction // z. B. (label)
    }

    public InstructionType GetInstructionType(string instruction)
    {
        return instruction[0] switch
        {
            '@' => InstructionType.AInstruction,
            '(' => InstructionType.LInstruction,
            _ => InstructionType.CInstruction
        };
    }

    // dest = comp ; jump

    public string GetCompPart(string instruction)
        => !instruction.Contains('=') ? instruction.Split(';')[0]
            : instruction.Split('=')[1];

    public string GetDestPart(string instruction)
        => !instruction.Contains('=') ? string.Empty
            : instruction.Split('=')[0];

    public string GetJumpPart(string instruction)
        => !instruction.Contains(';') ? string.Empty
                                      : instruction.Split(';')[1];
}
