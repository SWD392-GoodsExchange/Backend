using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Product;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Data.Models;
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
        public Task<PagedList<ReportDto>> GetReportsProcessing(ReportParam reportParam);
        public Task<PagedList<ReportDto>> GetReportsApproved(ReportParam reportParam);
        public Task<PagedList<ReportDto>> GetReportsByProduct(int pId, ReportParam reportParam);

        public Task<Report> GetReport(int reportId);
		public Task<ReportDto> AddReport(CreateReportRequest reportRequest);
		public Task<Report> UpdateReportStatus(int reportId);
        public Task DeleteReport(int reportId);
	}
}
