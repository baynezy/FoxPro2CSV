using Converter.Services;
using Xunit.OpenCategories;

namespace Converter.Core.UnitTests.Services;

public class FileServiceTests
{
    private readonly FileService _sut = new();
    
    private const string NonExistentFile = "non-existent-file.dbf";
    private readonly string _fileThatExists = $"{TestPath()}file-that-exists.dbf";
    // ReSharper disable once StringLiteralTypo
    private readonly string _validFileToParse = $"{TestPath()}rcppdrnd.DBF";
    
    [Fact]
    public void InvalidInputFile_WhenFileDoesNotExist_ReturnsTrue()
    {
        // act
        var result = _sut.InvalidInputFile(NonExistentFile);

        // assert
        result.Should()
            .BeTrue();
    }
    
    [Fact]
    public void InvalidInputFile_WhenFileExists_ReturnsFalse()
    {
        // act
        var result = _sut.InvalidInputFile(_fileThatExists);

        // assert
        result.Should()
            .BeFalse();
    }
    
    [Fact]
    public void CreateConnectionString_ReturnsExpectedValue()
    {
        // arrange
        const string inputFile = "input-file.dbf";
        var expected = $"Provider=VFPOLEDB.1;Data Source={Path.GetFullPath(inputFile)};Collating Sequence=general;";

        // act
        var result = _sut.CreateConnectionString(inputFile);

        // assert
        result.Should()
            .Be(expected);
    }
    
    [Fact]
    public void CreateConnectionString_WhenInputFileIsRelativePath_ReturnsConnectionStringWithFullPath()
    {
        // arrange
        var expected = $"Provider=VFPOLEDB.1;Data Source={Path.GetFullPath(_validFileToParse)};Collating Sequence=general;";

        // act
        var result = _sut.CreateConnectionString(_validFileToParse);

        // assert
        result.Should()
            .Be(expected);
    }
    
    [Fact]
    public void ConstructOutputFileName_ReturnsExpectedValue()
    {
        // arrange
        const string inputFile = "input-file.dbf";
        const string expected = "input-file.csv";

        // act
        var result = _sut.ConstructOutputFileName(inputFile);

        // assert
        result.Should()
            .Be(expected);
    }
    
    [Fact]
    [LocalTest]
    public void DbfFileToDataTable_WhenLoadingFile_ThenReturnsDataTable()
    {
        // act
        var result = _sut.DbfFileToDataTable(_validFileToParse);

        // assert
        result.Should()
            .NotBeNull();
    }
    
    [Fact]
    [LocalTest]
    public void DbfFileToDataTable_WhenLoadingFile_ThenReturnsDataTableWithRows()
    {
        // act
        var result = _sut.DbfFileToDataTable(_validFileToParse);

        // assert
        result.Rows.Should()
            .NotBeEmpty();
    }
    
    [Fact]
    [LocalTest]
    public void DbfFileToDataTable_WhenLoadingFile_ThenReturnsDataTableWithColumns()
    {
        // act
        var result = _sut.DbfFileToDataTable(_validFileToParse);

        // assert
        result.Columns.Should()
            .NotBeEmpty();
    }
    
    [Fact]
    [LocalTest]
    public Task DbfFileToDataTable_WhenLoadingFile_ThenShouldBeDeterministic()
    {
        // act
        var result1 = _sut.DbfFileToDataTable(_validFileToParse);
        var result2 = _sut.DbfFileToDataTable(_validFileToParse);

        // assert
        result1.Should()
            .BeEquivalentTo(result2);

        return Verify(result1);
    }
    
    [Fact]
    [LocalTest]
    public void DataTableToString_WhenCalledWithTextDelimiter_ThenShouldReturnPopulatedString()
    {
        // arrange
        var dataTable =  _sut.DbfFileToDataTable(_validFileToParse);

        // act
        var result1 = _sut.DataTableToString(dataTable, "%%%");
        var result2 = _sut.DataTableToString(dataTable, "\"");

        // assert
        result1.Should()
            .NotBeEquivalentTo(result2);
    }
    
    [Fact]
    [LocalTest]
    public Task DataTableToString_WhenCalledWithTextDelimiter_ThenShouldBeDeterministic()
    {
        // arrange
        var dataTable =  _sut.DbfFileToDataTable(_validFileToParse);

        // act
        var result1 = _sut.DataTableToString(dataTable, "%%%");
        var result2 = _sut.DataTableToString(dataTable, "%%%");

        // assert
        result1.Should()
            .Be(result2);

        return Verify(result1);
    }
    
    [Fact]
    [LocalTest]
    public void DataTableToString_WhenCalled_ThenShouldReturnPopulatedString()
    {
        // arrange
        var dataTable =  _sut.DbfFileToDataTable(_validFileToParse);

        // act
        var result = _sut.DataTableToString(dataTable, "\"");

        // assert
        result.Should()
            .NotBeNullOrEmpty();
    }
    
    [LocalTest]
    [Fact]
    public Task DataTableToString_WhenCalled_ThenShouldBeDeterministic()
    {
        // arrange
        var dataTable =  _sut.DbfFileToDataTable(_validFileToParse);

        // act
        var result1 = _sut.DataTableToString(dataTable, "\"");
        var result2 = _sut.DataTableToString(dataTable, "\"");

        // assert
        result1.Should()
            .Be(result2);
        
        return Verify(result1);
    }
    
    private static string TestPath()
    {
        const string route = @"..\..\..\TestFiles\";
        var environmentPath = Environment.GetEnvironmentVariable("TestPath");

        return environmentPath ?? route;
    }
}