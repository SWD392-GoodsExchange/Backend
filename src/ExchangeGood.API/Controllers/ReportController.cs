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
using Microsoft.AspNetCore.Mvc;

namespace ExchangeGood.API.Controllers
{
	public class ReportController : BaseApiController
	{
		private readonly IReportService _reportService;
		public ReportController(IReportService reportService)
		{
			_reportService = reportService;
		}

		[HttpGet]
		public async Task<IActionResult> GetReports([FromQuery] ReportParam reportParam)
		{
			var response = await _reportService.GetAllReports(reportParam);
			if (response != null)
			{
                return Ok(BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ReportDto>(response, response.CurrentPage, response.PageSize, response.TotalCount, response.TotalPages)));
			}
			return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG));
        }

		[HttpDelete("{id}")]
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
		public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest reportRequest)
        {
            var response = await _reportService.AddReport(reportRequest);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, response));
            }
			return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG));
		}


        [HttpPut("{reportId}")]
        public async Task<IActionResult> UpdateReport(int reportId)
        {
            var response = await _reportService.UpdateReportStatus(reportId);
            if (response != null)
            {
                return Ok(BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, response));
            }
            return BadRequest(BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG));
        }
    }
}
