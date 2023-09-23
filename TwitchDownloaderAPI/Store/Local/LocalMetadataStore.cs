using System.Collections.Concurrent;
using System.Text.Json;
using TwitchDownloaderAPI.Models;
using TwitchDownloaderAPI.Store.Interfaces;

namespace TwitchDownloaderAPI.Store.Local;

public class LocalMetadataStore : IMetadataStore
{
    private readonly string _filepath;
    private readonly ConcurrentDictionary<int, ChatLogMetadata> _metadataStore = new();
    
    public LocalMetadataStore(IWebHostEnvironment env)
    {
        _filepath = Path.Combine(env.ContentRootPath, "Resources", "LocalMetadata.json");
        if (File.Exists(_filepath))
        {
            try
            {
                var content = File.ReadAllText(_filepath);
                Console.WriteLine(content);
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
                };
                var deserializedData = JsonSerializer.Deserialize<ChatLogMetadata>(content, options);

                if (deserializedData != null)
                {
                    _metadataStore[deserializedData.VideoId] = deserializedData;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error reading json metadata file: {ex.Message}");
            }
        }
    }
    
    public Task AddMetadataAsync(ChatLogMetadata metadata)
    {
        _metadataStore[metadata.VideoId] = metadata;
        File.WriteAllText(_filepath, JsonSerializer.Serialize(_metadataStore));
        return Task.CompletedTask;
    }

    public Task<ChatLogMetadata?> GetMetadataAsync(int videoId)
    {
        _metadataStore.TryGetValue(videoId, out var metadata);
        return Task.FromResult(metadata);
    }

    public Task UpdateMetadataAsync(ChatLogMetadata metadata)
    {
        return AddMetadataAsync(metadata);
    }
}