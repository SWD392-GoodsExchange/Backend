using ExchangeGood.API.Extensions;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Repository.Repository;
using ExchangeGood.Service.Interfaces;
using ExchangeGood.Service.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers
{
	public class ReportsController : BaseApiController
	{
		private readonly IReportService _reportService;
		public ReportsController(IReportService reportService)
		{
			_reportService = reportService;
		}

		[HttpGet]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> GetReports([FromQuery] ReportParam reportParam)
		{
			var response = await _reportService.GetAllReports(reportParam);
			if (response != null)
			{
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ReportDto>(response, response.CurrentPage, response.PageSize, response.TotalCount, response.TotalPages)));
			}
			return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("approved")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> GetReportsApproved([FromQuery] ReportParam reportParam)
        {
            var response = await _reportService.GetReportsApproved(reportParam);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ReportDto>(response, response.CurrentPage, response.PageSize, response.TotalCount, response.TotalPages)));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("processing")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> GetReportsProcessing([FromQuery] ReportParam reportParam)
        {
            var response = await _reportService.GetReportsProcessing(reportParam);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ReportDto>(response, response.CurrentPage, response.PageSize, response.TotalCount, response.TotalPages)));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("rejected")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> GetReportsRejected([FromQuery] ReportParam reportParam)
        {
            var response = await _reportService.GetReportsRejected(reportParam);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ReportDto>(response, response.CurrentPage, response.PageSize, response.TotalCount, response.TotalPages)));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> GetReportsByProduct([FromQuery] ReportParam reportParam, int id)
        {
            var response = await _reportService.GetReportsByProduct(id, reportParam);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ReportDto>(response, response.CurrentPage, response.PageSize, response.TotalCount, response.TotalPages)));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var existingReport = await _reportService.GetReport(id);
            if (existingReport != null)
            {
                await _reportService.DeleteReport(id);
                return Ok(BaseResponse.Success(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, existingReport));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_DELETE_MSG));
            
		}

		[HttpPost]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest reportRequest)
        {
            var feId = User.GetFeID();
            reportRequest.FeId = feId;
            var response = await _reportService.AddReport(reportRequest);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response));
            }
			return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
		}


        [HttpPut("approved/{reportId}")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> UpdateReportStatusApproved(int reportId)
        {
            var response = await _reportService.UpdateReportStatusApproved(reportId);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
        }


        [HttpPut("rejected/{reportId}")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> UpdateReportStatus(int reportId)
        {
             var response = await _reportService.UpdateReportStatusRejected(reportId);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
        }

        [HttpGet("totalReports")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> GetTotalProduct()
        {
            var result = await _reportService.GetTotalReportsAsync();
            return result != 0
                ? Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result))
                : NotFound(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }
    }
}
