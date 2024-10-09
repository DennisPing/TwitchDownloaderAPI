using System.Net;
using Microsoft.AspNetCore.Mvc;
using TwitchDownloaderAPI.Models;
using TwitchDownloaderAPI.Store;
using TwitchDownloaderAPI.Tools;
using TwitchDownloaderCore;
using TwitchDownloaderCore.Options;
using TwitchDownloaderCore.Tools;
using TwitchDownloaderCore.TwitchObjects.Gql;

namespace TwitchDownloaderAPI.Controllers;

[Route("api/videos/{videoId:int}")]
[ApiController]
public class VideoController : ControllerBase
{
    private readonly ILogger<VideoController> _logger;
    
    public VideoController(ILogger<VideoController> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Get video metadata from Twitch Helix API
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    [HttpGet("metadata")]
    public async Task<ActionResult<GqlVideoResponse>> GetMetadata([FromRoute] int videoId)
    {
        _logger.LogInformation("Fetching metadata for video ID: {VideoId}", videoId);
        
        GqlVideoResponse? metadata;
        try
        {
            metadata = await TwitchHelper.GetVideoInfo(videoId);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new { Message = $"Internal server error: {ex.Message}" });
        }

        if (metadata == null)
        {
            return NotFound("Invalid VOD, deleted/expired VOD possibly");
        }

        return StatusCode((int)HttpStatusCode.Gone);
    }
}