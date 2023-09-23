using TwitchDownloaderAPI.Store.Interfaces;

namespace TwitchDownloaderAPI.Store.Local;

public class LocalChatLogStore : IChatLogStore
{
    private readonly string _folderpath;

    public LocalChatLogStore(IWebHostEnvironment env)
    {
        _folderpath = Path.Combine(env.ContentRootPath, "Resources", "LocalChatLogs");
        if (!Directory.Exists(_folderpath))
        {
            Directory.CreateDirectory(_folderpath);
        }
    }
    
    public Task AddChatLogAsync(int videoId, byte[] chatLogData)
    {
        var filepath = Path.Combine(_folderpath, videoId.ToString());
        File.WriteAllBytes(filepath, chatLogData);
        return Task.CompletedTask;
    }

    public Task<byte[]?> GetChatLogAsync(int videoId)
    {
        var filename = $"{videoId}.txt";
        var filepath = Path.Combine(_folderpath, filename);
        
        // If for some reason the file does not exist or is empty
        if (!File.Exists(filepath) || new FileInfo(filepath).Length == 0)
        {
            return Task.FromResult<byte[]?>(null);
        }

        var content = File.ReadAllBytes(filepath);
        return Task.FromResult<byte[]?>(content);
    }
}