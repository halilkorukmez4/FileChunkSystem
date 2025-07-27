using FileChunkSystem.Application.Dtos;
using FileChunkSystem.Application.Features.MergeFile;
using FileChunkSystem.Application.Features.UploadFile;
using FileChunkSystem.Application.Helpers.MimeType;
using FileChunkSystem.Infrastructure.Common;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(new CompactJsonFormatter(), "logs/errors.log", rollingInterval: RollingInterval.Day, shared: true)
    .CreateLogger();

AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
{
    var ex = args.ExceptionObject as Exception;
    Log.Fatal(ex, "UNHANDLED EXCEPTION!");
};

builder.Services.AddInfrastructureLayer(builder.Configuration);

using var host = builder.Build();

await host.StartAsync();

var mediator = host.Services.GetRequiredService<IMediator>();

string[] filePaths =
[
    "manzarafotograf.webp",
    "Doga-Manzara-Duvar-Kagidi-394-resim-10015.jpg"
];

var metadata = new Dictionary<string, string>
{
    { "UploadedBy", "ConsoleUser" },
    { "Source", "Program.cs" }
};

List<UploadFileRequestDto> uploadRequests = [];

foreach (var filePath in filePaths)
{
    var fileInfo = new FileInfo(filePath);

    if (!fileInfo.Exists)
    {
        Console.WriteLine($"Skipped: {filePath} does not exist.");
        continue;
    }

    var mimeType = MimeTypeHelper.GetMimeType(fileInfo.Name);

    var stream = File.OpenRead(filePath);

    uploadRequests.Add(new UploadFileRequestDto
    (
        stream,
        fileInfo.Name,
        mimeType,
        fileInfo.Length,
        metadata
    ));
}

var uploadCommand = new UploadFileCommandRequest(uploadRequests);

var uploadResults = await mediator.Send(uploadCommand);

var downloadRequests = uploadResults.Select(x => new DownloadFileRequestDto(x.FileId)).ToList();

var downloadCommand = new DownloadFileCommandRequest(downloadRequests);

var downloadedFiles = await mediator.Send(downloadCommand);

Directory.CreateDirectory("C:\\Temp");

foreach (var downloaded in downloadedFiles)
{
    var sanitizedFileName = Path.GetFileNameWithoutExtension(downloaded.FileName);

    var extension = Path.GetExtension(downloaded.FileName);

    var outputPath = Path.Combine("C:\\Temp", $"Downloaded_{sanitizedFileName}_{Guid.NewGuid():N}{extension}");

    await using var fileStream = File.Create(outputPath);

    await downloaded.Content.CopyToAsync(fileStream);
}

Log.CloseAndFlush();