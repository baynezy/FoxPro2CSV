using CommandLine;

namespace Converter;

public class Options
{
    [Value(0, MetaName = "inputFile", Required = true, HelpText = "Input file to be processed.")]
    public string InputFile { get; set; }
}