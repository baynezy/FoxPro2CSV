# FoxPro2CSV

## Description

This application will convert a FoxPro database to a CSV file.

## Contributing

### Commit Template

Please run the following to make sure your commit messages conform to the project's
standards.

```pwsh
git config --local commit.template .gitmessage
```

### Using Locally

#### Pre-requisites

- .Net 8.0 SDK (32-bit)
- [Microsoft Visual FoxPro OLE DB Provider 9.0](https://download.cnet.com/microsoft-ole-db-provider-for-visual-foxpro-9-0/3000-10254_4-10729530.html)

#### Running

```pwsh
dotnet run --project .\src\Converter\Converter.csproj "path-to-foxpro-database.DBF"
```

##### Optional Parameters

| Parameter       | Short Code | Description                                                |
|-----------------|------------|------------------------------------------------------------|
| --textDelimiter | -d         | The text delimiter to use for the CSV file. Default is `"` |

## License

[Apache 2.0](LICENSE.txt)
