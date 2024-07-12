using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Enum.Member;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Service.UseCase
{
    public class ReportService : IReportService
	{
		private readonly IProductRepository _productRepository;
		private readonly IReportRepository _reportRepository;
		private readonly IMemberRepository _memberRepository;

		public ReportService(IProductRepository productRepository, IReportRepository reportRepository, IMemberRepository memberRepository)
		{
			_productRepository = productRepository;
			_reportRepository = reportRepository;
			_memberRepository = memberRepository;
		}

        public async Task<ReportDto> AddReport(CreateReportRequest reportRequest)
        {
            var existingProduct = await _productRepository.GetProduct(reportRequest.ProductId);
            var existingMember = await _memberRepository.GetMemberById(reportRequest.FeId);
            if(existingProduct != null && existingMember != null) {
                var report = await _reportRepository.AddReport(reportRequest);
                return report;
            }
            return null;
        }

        public async Task DeleteReport(int reportId)
        {
            await _reportRepository.DeleteReport(reportId);
        }

        public async Task<PagedList<ReportDto>> GetAllReports(ReportParam reportParam)
        {
            return await _reportRepository.GetAllReports(reportParam);
        }

        public async Task<ReportDto> GetReport(int reportId)
        {
            return  await _reportRepository.GetReport(reportId);
        }

        public async Task<PagedList<ReportDto>> GetReportsApproved(ReportParam reportParam)
        {
            return await _reportRepository.GetReportsApproved(reportParam);
        }

        public async Task<PagedList<ReportDto>> GetReportsByProduct(int pId, ReportParam reportParam)
        {
            return await _reportRepository.GetReportsByProduct(pId, reportParam);
        }

        public async Task<PagedList<ReportDto>> GetReportsProcessing(ReportParam reportParam)
        {
            return await _reportRepository.GetReportsProcessing(reportParam);
        }

        public async Task<PagedList<ReportDto>> GetReportsRejected(ReportParam reportParam)
        {
            return await _reportRepository.GetReportsRejected(reportParam);
        }

        public async Task<ReportDto> UpdateReportStatusApproved(int reportId)
        {
            var report = await _reportRepository.GetReport(reportId);
            if (report != null)
            {
                return await _reportRepository.UpdateReportStatusApproved(reportId);
            }
            return null;
        }

        public async Task<ReportDto> UpdateReportStatusRejected(int reportId)
        {
            var report = await _reportRepository.GetReport(reportId);
            if (report != null)
            {
                return await _reportRepository.UpdateReportStatusRejected(reportId);
            }
            return null;
        }

    }
}
