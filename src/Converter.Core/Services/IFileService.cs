using System.Data;

namespace Converter.Services;

internal interface IFileService
{
    bool InvalidInputFile(string inputFilePath);
    string CreateConnectionString(string inputFilePath);
    string ConstructOutputFileName(string inputFilePath);
    void WriteOutputFile(string outputFilePath, string fileContents);
    DataTable DbfFileToDataTable(string inputFilePath);
    string DataTableToString(DataTable table, string textDelimiter);
}