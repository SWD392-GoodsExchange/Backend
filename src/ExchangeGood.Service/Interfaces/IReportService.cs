﻿using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.Interfaces
{
	public interface IReportService
	{
        public Task<PagedList<ReportDto>> GetAllReports(ReportParam reportParam);
        public Task<PagedList<ReportDto>> GetReportsProcessing(ReportParam reportParam);
        public Task<PagedList<ReportDto>> GetReportsApproved(ReportParam reportParam);
        public Task<PagedList<ReportDto>> GetReportsRejected(ReportParam reportParam);
        public Task<PagedList<ReportDto>> GetReportsByProduct(int pId, ReportParam reportParam);
        public Task<ReportDto> GetReport(int reportId);
        public Task<ReportDto> AddReport(CreateReportRequest reportRequest);
        public Task<ReportDto> UpdateReportStatusApproved(int reportId);
        public Task<ReportDto> UpdateReportStatusRejected(int reportId);
        Task<int> GetTotalReportsAsync();
        public Task DeleteReport(int reportId);
    }
}
