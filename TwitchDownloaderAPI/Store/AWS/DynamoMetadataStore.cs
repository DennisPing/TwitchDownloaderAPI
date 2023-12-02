using TwitchDownloaderAPI.Models;

namespace TwitchDownloaderAPI.Store.AWS;

public class DynamoMetadataStore : IMetadataStore
{
    public Task AddMetadataAsync(ChatLogMetadata metadata)
    {
        throw new NotImplementedException();
    }

    public Task<ChatLogMetadata?> GetMetadataAsync(int videoId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateMetadataAsync(ChatLogMetadata metadata)
    {
        throw new NotImplementedException();
    }
}