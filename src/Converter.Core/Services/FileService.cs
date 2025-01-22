using System.Data;
using System.Data.OleDb;
using System.Text;

namespace Converter.Services;

internal class FileService : IFileService
{
    public bool InvalidInputFile(string inputFilePath) => !File.Exists(inputFilePath);

    public string CreateConnectionString(string inputFilePath)
    {
        var path = Path.GetFullPath(inputFilePath);
        return $"Provider=VFPOLEDB.1;Data Source={path};Collating Sequence=general;";
    }

    public string ConstructOutputFileName(string inputFilePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(inputFilePath);
        var outputFile = $"{fileName}.csv";
        return outputFile;
    }

    public void WriteOutputFile(string outputFilePath, string fileContents)
    {
        File.WriteAllText(outputFilePath, fileContents);
    }

    public DataTable DbfFileToDataTable(string inputFilePath)
    {
        var path = Path.GetFullPath(inputFilePath);
        var sqlSelect = $"SELECT * FROM '{path}'";
#pragma warning disable CA1416
        using var connection = new OleDbConnection(CreateConnectionString(inputFilePath));
        using var da = new OleDbDataAdapter(sqlSelect, connection);
#pragma warning restore CA1416

        var ds = new DataSet();
        da.Fill(ds);
        return ds.Tables[0];
    }

    public string DataTableToString(DataTable table, string textDelimiter)
    {
        var sb = new StringBuilder();
        var columnNames = table.Columns.Cast<DataColumn>()
            .Select(column => column.ColumnName)
            .ToArray();
        sb.AppendLine(string.Join(",", columnNames));

        foreach (DataRow row in table.Rows)
        {
            var fields = row.ItemArray
                .Select(field => field!.ToString())
                .ToArray();
            for (var i = 0; i < fields.Length; i++)
            {
                sb.Append(textDelimiter + fields[i]
                    ?.Trim());
                sb.Append(IsLastFieldInRow(i, fields) ? textDelimiter : textDelimiter + ",");
            }

            sb.AppendLine();
        }
        
        return sb.ToString();
    }

    private static bool IsLastFieldInRow(int i, string?[] fields)
    {
        return i == fields.Length - 1;
    }
}