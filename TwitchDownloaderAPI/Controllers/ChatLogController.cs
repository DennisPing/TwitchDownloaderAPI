using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using TwitchDownloaderAPI.Models;
using TwitchDownloaderAPI.Store.Interfaces;

namespace TwitchDownloaderAPI.Controllers
{
    [Route("api/videos/{videoId}/chatlogs")]
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

        // GET: api/chatlog/{videoId}/metadata
        [HttpGet("metadata")]
        public async Task<ActionResult<ChatLogMetadata>> GetMetadata([FromRoute] int videoId)
        {
            try
            {
                var metadata = await _metadataStore.GetMetadataAsync(videoId);
                if (metadata != null)
                {
                    return Ok(metadata);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/chatlog/{videoId}/content
        [HttpGet("content")]
        [Produces("application/octet-stream", "application/x-protobuf")]
        public async Task<IActionResult> GetChatLog([FromRoute] int videoId)
        {
            try
            {
                var metadata = await _metadataStore.GetMetadataAsync(videoId);
                if(metadata == null)
                {
                    return NotFound(new { Message = $"Chat log metadata not found for videoId: {videoId}" });
                }

                var chatLogContent = await _chatLogStore.GetChatLogAsync(metadata.VideoId);
                if(chatLogContent == null || chatLogContent.Length == 0)
                {
                    return NotFound(new { Message = $"Chat log content not found for videoId: {videoId}" });
                }
        
                // Let the framework handle Content-Length header.
                // Let the framework decide the content type based on content negotiation.
                return File(chatLogContent, "application/octet-stream");
            }
            catch(Exception ex)
            {
                // Log the exception and return 500 Internal Server Error.
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
