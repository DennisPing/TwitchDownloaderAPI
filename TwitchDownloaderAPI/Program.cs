using Serilog;
using TwitchDownloaderAPI.Middlewares;
using TwitchDownloaderAPI.Store.Interfaces;
using TwitchDownloaderAPI.Store.Local;

/*
 * Rough draft routes
 *
 * api/videos/{videoId}/metadata - Video metadata
 * api/videos/{videoId}/content - Video content
 *
 * api/videos/{videoId}/chatlogs/metadata - Chatlog metadata related to video
 * api/videos/{videoId}/chatlogs/content - Chatlog content related to video
 *
 * api/videos/{videoId}/chatlogs/analytics - Analytical data related to chatlog
 * api/videos/[videoId}/chatlogs/graphs - Graphical data related to chatlog
 */

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
});
builder.Services.AddScoped<IMetadataStore, LocalMetadataStore>();
builder.Services.AddScoped<IChatLogStore, LocalChatLogStore>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog enrichers
// builder.Host.UseSerilog((context, configuration) =>
// {
//     configuration
//         .ReadFrom.Configuration(context.Configuration)
//         .Enrich.FromLogContext()
//         .Enrich.WithCorrelationIdHeader();
// }, writeToProviders: true);

builder.Host.UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration));

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine("Environment: " + app.Environment.EnvironmentName); // Check the environment
Console.WriteLine("IsDevelopment: " + app.Environment.IsDevelopment()); // Check if it's development
Console.WriteLine("Base Directory: " + AppDomain.CurrentDomain.BaseDirectory); // Check the base directory

app.UseHttpsRedirection();
// app.UseMiddleware<LoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();

Log.CloseAndFlush();