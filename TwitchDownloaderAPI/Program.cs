using Serilog;
using Serilog.Formatting.Compact;
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

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(new CompactJsonFormatter()) // for local development
    .WriteTo.File(new CompactJsonFormatter(), "./logs/log-.txt", 
        rollingInterval: RollingInterval.Day, 
        retainedFileCountLimit: 30) // retains log files for 30 days
    .CreateLogger();

builder.Host.UseSerilog();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

Log.CloseAndFlush();