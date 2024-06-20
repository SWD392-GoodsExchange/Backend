using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces
{
	public interface IReportService
	{
		public Task<BaseResponse> GetAllReports(ReportParam reportParams);
		public Task<BaseResponse> GetReport(int ReportId);
		public Task<BaseResponse> AddReport(CreateReportRequest createreportRequest);
		public Task<BaseResponse> UpdateReport(UpdateReportRequest updatereportRequest);
		public Task<BaseResponse> DeleteReport(int ReportId);
	}
}
