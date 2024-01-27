namespace TwitchDownloaderAPI.Models;

public class ChatLogMetadata
{
    public int ChatLogId { get; set; }
    public int VideoId { get; set; }
    public int StreamerId { get; set; }
    public string? StreamerHandle { get; set; }
    public string? StreamerName { get; set; }
    public int NumMessages { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StreamLength { get; set; }
}