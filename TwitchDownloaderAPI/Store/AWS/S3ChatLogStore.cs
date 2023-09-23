using TwitchDownloaderAPI.Store.Interfaces;

namespace TwitchDownloaderAPI.Store.AWS;

public class S3ChatLogStore : IChatLogStore
{
    public Task AddChatLogAsync(int videoId, byte[] chatLogData)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]?> GetChatLogAsync(int videoId)
    {
        throw new NotImplementedException();
    }
}