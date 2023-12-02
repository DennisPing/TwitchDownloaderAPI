using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TwitchDownloaderAPI.Models;
using TwitchDownloaderAPI.Store;

namespace TwitchDownloaderAPI.Controllers
{
    [Route("api/videos/{videoId:int}/chatlog")]
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
        
        [HttpGet("content")]
        [Produces("text/plain", "application/x-protobuf")]
        public async Task<IActionResult> GetChatLog([FromRoute] int videoId)
        {
            byte[]? chatLogContent;
            try
            {
                chatLogContent = await _chatLogStore.GetChatLogAsync(videoId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
            
            if (chatLogContent == null || chatLogContent.Length == 0)
            {
                return NotFound(new { Message = $"Chat log not found for videoId: {videoId}" });
            }
            return File(chatLogContent, "text/plain");
        }
    }
}
