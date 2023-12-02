namespace TwitchDownloaderAPI.Store;

public interface IChatLogStore
{
    string ChatLogLocation { get; }
    Task AddChatLogAsync(int videoId, byte[] chatLogData);
    Task<byte[]?> GetChatLogAsync(int videoId);
}