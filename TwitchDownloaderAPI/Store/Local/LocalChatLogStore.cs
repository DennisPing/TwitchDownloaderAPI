namespace TwitchDownloaderAPI.Store.Local;

public class LocalChatLogStore : IChatLogStore
{
    private readonly string _folderpath;

    public LocalChatLogStore(IHostEnvironment env)
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

    /// <summary>
    /// Asynchronously return the local chat log data
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    public async Task<byte[]?> GetChatLogAsync(int videoId)
    {
        var filename = $"{videoId}_chat.txt";
        var filepath = Path.Combine(_folderpath, filename);
        
        // If for some reason the file does not exist or is empty
        if (!File.Exists(filepath) || new FileInfo(filepath).Length == 0)
        {
            return null;
        }

        var content = await File.ReadAllBytesAsync(filepath);
        return content;
    }
}