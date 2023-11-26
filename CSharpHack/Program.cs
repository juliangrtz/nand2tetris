namespace HackAssembler;

/// <summary>
/// Ein Assembler für die Hack-Assemblysprache.
/// </summary>
internal class Program
{
    private const string AssemblyFileExtension = ".asm";
    private const string HackFileExtension = ".hack";
    private const string PreprocessedFileExtension = ".preprocessed";

    public static void ShowUsageAndQuit()
    {
        Console.WriteLine($"Usage: HackAssembler.exe <filename>{AssemblyFileExtension}");
        Console.WriteLine($"The resulting {HackFileExtension} file is being written to <filename>{HackFileExtension}.");
        Environment.Exit(-1);
    }

    public static void ShowErrorAndQuit(string errorMsg)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(errorMsg);
        Console.ResetColor();
        Environment.Exit(-1);
    }

    private static void CheckFile(string filename)
    {
        if (!filename.EndsWith(AssemblyFileExtension))
            ShowErrorAndQuit($"Input file must be a {AssemblyFileExtension} file!");

        if (!File.Exists(filename))
            ShowErrorAndQuit($"File {filename} does not exist!");
    }

    private static void Main(string[] args)
    {
        if (args.Length < 1)
            ShowUsageAndQuit();

        var asmFilePath = args[0];
        CheckFile(asmFilePath);

        var asmFileContent = File.ReadAllLines(asmFilePath);

        Console.WriteLine($"Preprocessing {asmFilePath}...");
        var preprocessed = new Preprocessor().Preprocess(asmFileContent);

#if DEBUG
        File.WriteAllLines(asmFilePath + PreprocessedFileExtension, preprocessed);
#endif

        Console.WriteLine("Assembling the preprocessed file...");
        var assembled = new Assembler(preprocessed).Assemble();

        Console.WriteLine("Writing output...");
        var outputFile = asmFilePath.Replace(AssemblyFileExtension, HackFileExtension);
        File.WriteAllLines(outputFile, assembled);

        Console.WriteLine("Done!");
    }
}
