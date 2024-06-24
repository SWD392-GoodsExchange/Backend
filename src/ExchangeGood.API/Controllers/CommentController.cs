using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Request.Comment;
using ExchangeGood.Service.Interfaces;
using ExchangeGood.Service.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers
{
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _CommentService;
        // private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService)
        {
            _CommentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommets()
        {
            var response = await _CommentService.GetAllComments();
            if (response.Data != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest commentRequest)
        {
            var response = await _CommentService.AddComment(commentRequest);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var response = await _CommentService.DeleteComment(id);
            return Ok(response);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest updateComment)
        {
            var response = await _CommentService.UpdateComment(updateComment);
            return Ok(response);
        }
    }
}
