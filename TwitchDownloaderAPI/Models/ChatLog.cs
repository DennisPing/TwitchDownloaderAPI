namespace TwitchDownloaderAPI.Models;

public class ChatLog
{
    public int ChatLogId { get; set; }
    public byte[] Content { get; set; } // Serialized and compressed chat log
}