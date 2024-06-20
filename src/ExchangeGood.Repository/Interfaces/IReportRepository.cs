using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Request.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Interfaces
{
	public interface IReportRepository
	{
		public Task<PagedList<ReportDto>> GetAllReports(ReportParam reportParam);
		public Task<ReportDto> GetReport(int reportId);
		public Task<ReportDto> AddReport(CreateReportRequest reportRequest);
		public Task<int> UpdateReport(UpdateReportRequest reportRequest);
		public Task DeleteReport(int reportId);
	}
}
