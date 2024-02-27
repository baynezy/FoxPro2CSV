using System.Data;
using System.Data.OleDb;
using System.Text;
using Cocona;
using Microsoft.Extensions.Logging;

namespace Converter.Commands.ConvertDbfToCsv;

internal class ConvertDbfToCsvCommand(ILogger<ConvertDbfToCsvCommand> logger)
{
    public int Convert([Argument] string inputFile)
    {
        logger.LogInformation("Input file: {InputFile}", inputFile);

        if (InvalidInputFile(inputFile))
        {
            logger.LogError("Input file {InputFile} does not exist", inputFile);
            return -1;
        }
        
        var connectionString = $"Provider=VFPOLEDB.1;Data Source={inputFile};Collating Sequence=general;";
        var fileName = Path.GetFileNameWithoutExtension(inputFile);
        var outputFile = $"{fileName}.csv";
        logger.LogInformation("Output file: {OutputFile}", outputFile);
        
        ConvertToCsv(connectionString, inputFile, outputFile);

        return 0;
    }

    private static bool InvalidInputFile(string inputFile)
    {
        return !File.Exists(inputFile);
    }

    private void ConvertToCsv(string connectionString, string inputFile, string outputFile)
    {
        var sqlSelect = $"SELECT * FROM '{inputFile}'";

#pragma warning disable CA1416
        using var connection = new OleDbConnection(connectionString);
        using var da = new OleDbDataAdapter(sqlSelect, connection);
#pragma warning restore CA1416
        var ds = new DataSet();
        da.Fill(ds);
        DataTableToCsv(ds.Tables[0], outputFile);
        logger.LogInformation("Converted {InputFile} to {OutputFile}", inputFile, outputFile);
    }

    private static void DataTableToCsv(DataTable table, string outputFile)
    {
        var sb = new StringBuilder();
        var columnNames = table.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
        sb.AppendLine(string.Join(",", columnNames));
        
        foreach (DataRow row in table.Rows)
        {
            var fields = row.ItemArray.Select(field => field!.ToString()).ToArray();
            for (var i = 0; i < fields.Length; i++)
            {
                sb.Append("\"" + fields[i]?.Trim());
                sb.Append(i == fields.Length - 1 ? "\"" : "\",");
            }
            sb.AppendLine();
        }
        File.WriteAllText(outputFile, sb.ToString());
    }
}