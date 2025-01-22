using Cocona;
using Converter.Services;
using Microsoft.Extensions.Logging;

namespace Converter.Commands.ConvertDbfToCsv;

internal class ConvertDbfToCsvCommand(IFileService fileService, ILogger<ConvertDbfToCsvCommand> logger)
{
    public int Convert(
        [Argument(Description = "The location of the .dbx file we are processing.")] string inputFile,
        [Option('d', Description = "The text delimiter to use in the CSV output file.")] string textDelimiter = "\"")
    {
        logger.LogInformation("Input file: {InputFile}", inputFile);

        if (fileService.InvalidInputFile(inputFile))
        {
            logger.LogError("Input file {InputFile} does not exist", inputFile);
            return -1;
        }

        var outputFile = fileService.ConstructOutputFileName(inputFile);
        logger.LogInformation("Output file: {OutputFile}", outputFile);

        ConvertToCsv(inputFile, outputFile, textDelimiter);

        return 0;
    }

    private void ConvertToCsv(string inputFile, string outputFile, string textDelimiter)
    {
        var dataTable = fileService.DbfFileToDataTable(inputFile);
        var data = fileService.DataTableToString(dataTable, textDelimiter);
        logger.LogInformation("Converted {InputFile} to {OutputFile}", inputFile, outputFile);
        fileService.WriteOutputFile(outputFile, data);
    }
}