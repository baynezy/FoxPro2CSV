using Cocona;
using Converter.Commands.ConvertDbfToCsv;
using Converter.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;

var builder = CoconaApp.CreateBuilder();

builder.Logging.ClearProviders();
builder.Logging.AddSpectreConsole(config =>
{
    config.AddTemplateRenderers();

    const string generalTemplate    = "[grey85][[{DateTime:T} [green3_1]{LogLevel}[/]]] {Message}{NewLine}{Exception}[/]";
    const string infoTemplate       = "[[{DateTime:T} [green3_1]Info[/]]] {Message}{NewLine}{Exception}";
    const string errorTemplate      = "[[{DateTime:T} [red bold]Error[/]]] {Message}{NewLine}{Exception}";
    const string criticalTemplate   = "[[{DateTime:T} [red bold]Critical[/]]] {Message}{NewLine}{Exception}";
                    
    config.ConfigureProfiles([
        LogLevel.Information
    ], profile => profile.OutputTemplate = infoTemplate);
    config.ConfigureProfiles([
        LogLevel.Error
    ], profile => profile.OutputTemplate = errorTemplate);
    config.ConfigureProfiles([
        LogLevel.Critical
    ], profile => profile.OutputTemplate = criticalTemplate);
    config.ConfigureProfiles([
        LogLevel.Debug,
        LogLevel.Trace,
        LogLevel.Warning
    ],profile => profile.OutputTemplate = generalTemplate);
});

builder.Services.AddSingleton<ConvertDbfToCsvCommand>();
builder.Services.AddTransient<IFileService, FileService>();

var app = builder.Build();

Console.WriteLine("Visual FoxPro 2 CSV Converter v1.1.5");
Console.WriteLine("=====================================");
Console.WriteLine();

app.AddCommands<ConvertDbfToCsvCommand>();

await app.RunAsync();