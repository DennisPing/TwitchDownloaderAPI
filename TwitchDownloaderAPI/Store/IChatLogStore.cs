namespace TwitchDownloaderAPI.Store;

public interface IChatLogStore
{
    Task AddChatLogAsync(int videoId, byte[] chatLogData);
    Task<byte[]?> GetChatLogAsync(int videoId);
}