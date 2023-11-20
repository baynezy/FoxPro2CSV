// See https://aka.ms/new-console-template for more information

using CommandLine;
using Converter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options =>
    {
        Console.WriteLine("Visual FoxPro 2 CSV Converter v1.0.0");
        Console.WriteLine("=====================================");
        Console.WriteLine();
        
        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(options);
            services.AddSingleton<ConverterService>();
        });
        
        var app = builder.Build();
        
        var converter = app.Services.GetRequiredService<ConverterService>();
        converter.Convert();
    });


