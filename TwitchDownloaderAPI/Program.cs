using Serilog;
using TwitchDownloaderAPI.Store;
using TwitchDownloaderAPI.Store.Local;

/*
 * Rough draft routes
 *
 * api/chatlogs/{videoId}/metadata - chatlog metadata
 * api/chatlogs/{videoId}/content - chatlog content
 *
 * api/videos/{videoId}/metadata - video metadata
 * api/videos/{videoId}/content - video content
 *
 * api/analytics/{videoId}/analytics - analytical data related to chatlog
 * api/graphs/[videoId}/graphs - graphical data related to chatlog
 */

var builder = WebApplication.CreateBuilder(args);

// === 1. Serilog configuration ===
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

Log.Logger.Information("Test log entry from Program.cs");

// === 2. Dependency injection ===
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
});
builder.Services.AddScoped<IMetadataStore, LocalMetadataStore>();
builder.Services.AddScoped<IChatLogStore, LocalChatLogStore>();
builder.Services.AddProblemDetails();

// === 3. Swagger services ===
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// === 4. Build web app ===
var app = builder.Build();

// === 5. Conditional middleware configuration ===
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

// === 6. General middleware configuration ===
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

// === 7. Graceful shutdown ===
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();