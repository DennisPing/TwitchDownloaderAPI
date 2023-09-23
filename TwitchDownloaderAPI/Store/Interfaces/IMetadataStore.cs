using TwitchDownloaderAPI.Models;

namespace TwitchDownloaderAPI.Store.Interfaces;

public interface IMetadataStore
{
    Task AddMetadataAsync(ChatLogMetadata metadata);
    Task<ChatLogMetadata?> GetMetadataAsync(int videoId);
    Task UpdateMetadataAsync(ChatLogMetadata metadata);
}