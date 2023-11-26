using HackAssembler.Macros;

namespace HackAssembler;

/// <summary>
/// Ein Präprozessor für den Assembler, der Kommentare, Leerzeilen und Leerzeichen
/// im Eingabeprogramm entfernt und Makros übersetzt.
/// </summary>
public class Preprocessor
{
    private const string KommentarSymbol = "//";

    private static bool IsBlankLine(string line)
        => line.Equals(string.Empty) || string.IsNullOrWhiteSpace(line);

    private static bool BeginsWithComment(string line)
        => line.StartsWith(KommentarSymbol);

    private string[] ReplaceMacrosWithAssembly(IEnumerable<string> hackProgram)
    {
        var lines = new List<string>();

        foreach (var line in hackProgram)
        {
            // Todo: Reflection benutzen
            if (line.StartsWith("goto"))
            {
                var gotoMacro = new GotoMacro(line);
                lines.AddRange(gotoMacro.GetAssemblyCode());
            }
            else if (line.StartsWith("exit"))
            {
                var exitMacro = new ExitMacro(line);
                lines.AddRange(exitMacro.GetAssemblyCode());
            }
            else
            {
                lines.Add(line);
            }
        }

        return lines.ToArray();
    }

    private string[] RemoveCommentsAndWhitespaces(IEnumerable<string> hackProgram)
    {
        List<string> normalized = new();

        foreach (var line in hackProgram)
        {
            var trimmed = line.Trim();

            // Handelt es sich um eine Leerzeile oder eine Zeile, die mit einem Kommentar beginnt?
            if (IsBlankLine(trimmed) || BeginsWithComment(trimmed))
                continue;

            // Enthält die Zeile einen Kommentar nach der Instruktion?
            if (trimmed.Contains(KommentarSymbol))
            {
                var substring = trimmed[..trimmed.IndexOf('/')];
                normalized.Add(substring.Trim());
            }
            else
            {
                normalized.Add(trimmed.Trim());
            }
        }

        return normalized.ToArray();
    }

    public string[] Preprocess(string[] hackProgram)
    {
        // Erst Kommentare und Leerzeichen entfernen, dann Makros ersetzen
        var firstPass = RemoveCommentsAndWhitespaces(hackProgram);
        var secondPass = ReplaceMacrosWithAssembly(firstPass);

        return secondPass;
    }
}
