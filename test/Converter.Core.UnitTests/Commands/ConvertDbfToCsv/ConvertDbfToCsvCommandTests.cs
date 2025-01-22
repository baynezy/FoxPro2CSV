using Converter.Commands.ConvertDbfToCsv;
using Converter.Services;
using Microsoft.Extensions.Logging;

namespace Converter.Core.UnitTests.Commands.ConvertDbfToCsv;

public class ConvertDbfToCsvCommandTests
{
    private readonly IFileService _mockFileService = Substitute.For<IFileService>();
    private readonly ILogger<ConvertDbfToCsvCommand> _mockLogger = Substitute.For<ILogger<ConvertDbfToCsvCommand>>();

    private readonly ConvertDbfToCsvCommand _sut;

    public ConvertDbfToCsvCommandTests()
    {
        _sut = new ConvertDbfToCsvCommand(_mockFileService, _mockLogger);
    }

    [Fact]
    public void Convert_InvalidInputFile_ReturnsMinusOne()
    {
        // arrange
        _mockFileService
            .InvalidInputFile(Arg.Any<string>())
            .Returns(true);

        // act
        var result = _sut.Convert("inputFile.dbf");

        // assert
        result.Should()
            .Be(-1);
    }

    [Fact]
    public void Convert_ValidInputFile_CallsConvertToCsv()
    {
        // arrange
        _mockFileService
            .InvalidInputFile(Arg.Any<string>())
            .Returns(false);

        // act
        _sut.Convert("inputFile.dbf");

        // assert
        _mockFileService
            .Received(1)
            .DbfFileToDataTable("inputFile.dbf");
    }
    
    [Fact]
    public void Convert_ValidInputFile_CallsWriteOutputFile()
    {
        // arrange
        _mockFileService
            .InvalidInputFile(Arg.Any<string>())
            .Returns(false);

        // act
        _sut.Convert("inputFile.dbf");

        // assert
        _mockFileService
            .Received(1)
            .WriteOutputFile(Arg.Any<string>(), Arg.Any<string>());
    }
    
    [Fact]
    public void Convert_ValidInputFile_ReturnsZero()
    {
        // arrange
        _mockFileService
            .InvalidInputFile(Arg.Any<string>())
            .Returns(false);

        // act
        var result = _sut.Convert("inputFile.dbf");

        // assert
        result.Should()
            .Be(0);
    }
}