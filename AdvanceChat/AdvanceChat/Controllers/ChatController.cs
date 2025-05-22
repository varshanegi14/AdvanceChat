using AdvanceChat.Repositories;
using ChatModels;
using ChatModels.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdvanceChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        public IChatRepository _chatRepository;
        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        [HttpGet("group-chat")]
        public async Task<ActionResult<List<GroupChatDTO>>> GetChatsAsync()
        {
            return Ok(await _chatRepository.GetChatsAsync());
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAvailableUsersAsync()
        {
            return Ok(await _chatRepository.GetAvailableUserAsync());
        }

        [HttpPost("individual")]
        public async Task<IActionResult> GetIndividualChatAsync(RequestChatDTO requestChatDTO)
        {
            return Ok(await _chatRepository.GetIndividualChatsAsync(requestChatDTO));
        }

        [HttpGet("id")]
        public async Task<IActionResult> FindNameById(string id)
        {
            return Ok(await _chatRepository.findNameByID(id));
        }

    }
}
