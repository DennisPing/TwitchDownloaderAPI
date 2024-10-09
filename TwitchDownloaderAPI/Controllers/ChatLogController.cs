using System.Net;
using Microsoft.AspNetCore.Mvc;
using TwitchDownloaderAPI.Models;
using TwitchDownloaderAPI.Store;
using TwitchDownloaderAPI.Tools;
using TwitchDownloaderCore;
using TwitchDownloaderCore.Options;
using TwitchDownloaderCore.Tools;

namespace TwitchDownloaderAPI.Controllers;

[Route("api/chatlogs/{videoId:int}")]
[ApiController]
public class ChatLogController : ControllerBase
{
    private readonly IMetadataStore _metadataStore;
    private readonly IChatLogStore _chatLogStore;
    private readonly ILogger<ChatLogController> _logger;

    public ChatLogController(IMetadataStore metadataStore, IChatLogStore chatLogStore, ILogger<ChatLogController> logger)
    {
        _metadataStore = metadataStore;
        _chatLogStore = chatLogStore;
        _logger = logger;
    }

    /// <summary>
    /// Get chatlog metadata from local store
    /// </summary>
    /// <param name="videoId">The video ID</param>
    /// <returns></returns>
    [HttpGet("metadata")]
    public async Task<ActionResult<ChatLogMetadata>> GetMetadata([FromRoute] int videoId)
    {
        ChatLogMetadata? metadata;
        try
        {
            metadata = await _metadataStore.GetMetadataAsync(videoId);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new { Message = $"Internal server error: {ex.Message}" });
        }

        if (metadata == null || metadata.ChatLogId == 0)
        {
            return NotFound();
        }

        return Ok(metadata);
    }
    
    /// <summary>
    /// Get chatlog content if in local store, else call the chatlog downloader
    /// </summary>
    /// <param name="videoId">The video ID</param>
    /// <returns></returns>
    [HttpGet("content")]
    public async Task<IActionResult> GetChatLog([FromRoute] int videoId)
    {
        byte[]? chatLogContent;
        try
        {
            chatLogContent = await _chatLogStore.GetChatLogAsync(videoId);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new { Message = $"An error occurred: {ex.Message}" });
        }
        
        if (chatLogContent == null || chatLogContent.Length == 0)
        {
            return NotFound(new { Message = $"Chat log not found for videoId: {videoId}" });
        }

        return File(chatLogContent, "text/plain");
    }

    /// <summary>
    /// Download the chatlog for a Twitch VOD
    /// </summary>
    /// <param name="videoId">The video ID</param>
    /// <returns></returns>
    [HttpGet("download")]
    public async Task<IActionResult> DownloadChatLog([FromRoute] int videoId)
    {
        var opts = new ChatDownloadOptions()
        {
            DownloadFormat = ChatFormat.Text,
            Compression = ChatCompression.None,
            Id = videoId.ToString(),
            Filename = Path.Combine(_chatLogStore.ChatLogLocation, $"{videoId}_chat.txt"),
            TimeFormat = TimestampFormat.Relative,
        };

        var progress = new ApiTaskProgress(_logger);
        var chatDownloader = new ChatDownloader(opts, progress);
        try
        {
            await chatDownloader.DownloadAsync(new CancellationToken());
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new { Message = $"Error downloading {videoId}: {ex.Message}" });
        }

        byte[]? chatLogContent;
        try
        {
            chatLogContent = await _chatLogStore.GetChatLogAsync(videoId);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new { Message = $"An error occurred: {ex.Message}" });
        }
        
        if (chatLogContent == null || chatLogContent.Length == 0)
        {
            return NotFound(new { Message = $"Chat log not found for videoId: {videoId}" });
        }

        return File(chatLogContent, "text/plain");
    }
    
}