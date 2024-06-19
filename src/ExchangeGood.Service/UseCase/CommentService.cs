using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Comment;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeGood.Repository.Repository;

namespace ExchangeGood.Service.UseCase
{
    internal class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<BaseResponse> AddComment(CreateCommentRequest createComment)
        {

            await _commentRepository.AddComment(createComment);
            return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, createComment);
        }

        public async Task<BaseResponse> DeleteComment(int id)
        {
            var result = await _commentRepository.GetCommentByID(id);

            if (result != null)
            {
                await _commentRepository.DeleteComment(id);
                return BaseResponse.Success(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
            }
            else
            {
                return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_DELETE_MSG);
            }
        }

        public async Task<BaseResponse> GetAllComments()
        {
            var result = await _commentRepository.GetAllComments();
            return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new List<CommentDto>(result));
        }

        public async Task<BaseResponse> GetCommentByID(int id)
        {
            var result = await _commentRepository.GetCommentByID(id);

            if (result != null)
            {
                return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }

            return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
        }

        public async Task<BaseResponse> UpdateComment(UpdateCommentRequest updateComment)
        {
            var result = await _commentRepository.GetCommentByID(updateComment.CommentId);

            if (result != null)
            {
                await _commentRepository.UpdateComment(updateComment);
                return BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, updateComment);
            }
            else
            {
                return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
    }
}
