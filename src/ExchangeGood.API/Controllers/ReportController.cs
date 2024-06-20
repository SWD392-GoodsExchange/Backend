using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.Payloads.Request.Category;
using ExchangeGood.Contract.Payloads.Request.Report;
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
			if (response.Data != null)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteReport(int id)
		{
			var response = await _reportService.DeleteReport(id);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest reportRequest)
		{
			var response = await _reportService.AddReport(reportRequest);
			return Ok(response);
		}


		[HttpPut]
		public async Task<IActionResult> UpdateReport([FromBody] UpdateReportRequest reportRequest)
		{
			var response = await _reportService.UpdateReport(reportRequest);
			return Ok(response);
		}


	}
}
