using Serilog;
using TwitchDownloaderAPI.Store;
using TwitchDownloaderAPI.Store.Local;

/*
 * Rough draft routes
 *
 * api/videos/{videoId}/metadata - Video metadata
 * api/videos/{videoId}/content - Video content
 *
 * api/videos/{videoId}/chatlog/metadata - Chatlog metadata related to video
 * api/videos/{videoId}/chatlog/content - Chatlog content related to video
 *
 * api/videos/{videoId}/chatlog/analytics - Analytical data related to chatlog
 * api/videos/[videoId}/chatlog/graphs - Graphical data related to chatlog
 */

var builder = WebApplication.CreateBuilder(args);

// === 1. Serilog configuration ===
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// === 2. Dependency injection ===
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
});
builder.Services.AddScoped<IMetadataStore, LocalMetadataStore>();
builder.Services.AddScoped<IChatLogStore, LocalChatLogStore>();

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