using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace Converter;

public class ConverterService(Options options, ILogger<ConverterService> logger)
{
    public void Convert()
    {
        var inputFile = options.InputFile;
        logger.LogInformation("Input file: {InputFile}", inputFile);
        
        var connectionString = $"Provider=VFPOLEDB.1;Data Source={inputFile};Collating Sequence=general;";
        var fileName = Path.GetFileNameWithoutExtension(inputFile);
        var outputFile = $"{fileName}.csv";
        logger.LogInformation("Output file: {OutputFile}", outputFile);
        
        ConvertToCsv(connectionString, inputFile, outputFile);
    }

    private void ConvertToCsv(string connectionString, string inputFile, string outputFile)
    {
        var sqlSelect = $"SELECT * FROM {inputFile}";

#pragma warning disable CA1416
        using var connection = new OleDbConnection(connectionString);
        using var da = new OleDbDataAdapter(sqlSelect, connection);
#pragma warning restore CA1416
        var ds = new DataSet();
        da.Fill(ds);
        DataTableToCsv(ds.Tables[0], outputFile);
    }

    private void DataTableToCsv(DataTable table, string outputFile)
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