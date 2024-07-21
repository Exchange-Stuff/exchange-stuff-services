using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.GroupChat;
using ExchangeStuff.Service.Models.MessageChat;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        //[ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("get-list-joined/{id}")]
        public async Task<IActionResult> GetListJoined([FromRoute] Guid id)
        {
            return Ok(new ResponseResult<List<GroupChatViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _chatService.GetGroupsJoined(id)
            });
        }
        //[ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("get-list-messages/{id}")]
        public async Task<IActionResult> ListMessages([FromRoute] Guid id)
        {
            return Ok(new ResponseResult<List<MessageChatViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _chatService.GetListMessage(id)
            });
        }
        //[ESAuthorize(new string[] { ActionConstant.READ })]
        [HttpGet("check-group")]
        public async Task<IActionResult> CheckGroupExisting(Guid senderId, Guid receiverId)
        {
            return Ok(new ResponseResult<GroupChatViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _chatService.CheckExisting(senderId, receiverId)
            });
        }
    }
}
