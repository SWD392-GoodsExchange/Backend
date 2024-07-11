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
        public Task<Report> GetReport(int reportId);
        public Task<ReportDto> AddReport(CreateReportRequest reportRequest);
        public Task<Report> UpdateReportStatus(int reportId);
        public Task DeleteReport(int reportId);
    }
}
