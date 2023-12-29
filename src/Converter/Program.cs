using Cocona;
using Converter.Commands.ConvertDbfToCsv;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();
builder.Services.AddSingleton<ConvertDbfToCsvCommand>();

var app = builder.Build();

Console.WriteLine("Visual FoxPro 2 CSV Converter v1.0.0");
Console.WriteLine("=====================================");
Console.WriteLine();

app.AddCommands<ConvertDbfToCsvCommand>();

app.Run();