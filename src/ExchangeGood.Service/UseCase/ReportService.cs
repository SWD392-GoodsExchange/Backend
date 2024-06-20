using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Response;
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
		public async Task<BaseResponse> AddReport(CreateReportRequest createreportRequest)
		{
			var existingProduct = await _productRepository.GetProduct(createreportRequest.ProductId);
			var existingMember = await _memberRepository.GetMemberById(createreportRequest.FeId);

			if (existingProduct != null && existingMember != null)
			{
				ReportDto reportdto = new ReportDto();
				reportdto = await _reportRepository.AddReport(createreportRequest);


				return BaseResponse.Success(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, reportdto);
			}
			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_CREATE_MSG);
		}


		public async Task<BaseResponse> DeleteReport(int ReportId)
		{
			var existingReport = await _reportRepository.GetReport(ReportId);
			if (existingReport != null)
			{
				await _reportRepository.DeleteReport(ReportId);
				return BaseResponse.Success(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, existingReport);
			}
			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_DELETE_MSG);
		}

		public async Task<BaseResponse> GetAllReports(ReportParam reportParams)
		{
			var result = await _reportRepository.GetAllReports(reportParams);

			if (result.TotalCount > 0)
			{
				return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, new PaginationResponse<ReportDto>(result, result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
			}

			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
		}

		public async Task<BaseResponse> GetReport(int ReportId)
		{
			var exstingReport = await _reportRepository.GetReport(ReportId);
			if (exstingReport != null)
			{
				return BaseResponse.Success(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, exstingReport);
			}
			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_READ_MSG);
		}

		public async Task<BaseResponse> UpdateReport(UpdateReportRequest updatereportRequest)
		{
			var report = await _reportRepository.GetReport(updatereportRequest.ReportId);
			if (report != null)
			{
				await _reportRepository.UpdateReport(updatereportRequest);
				return BaseResponse.Success(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, updatereportRequest);
			}
			return BaseResponse.Failure(Const.FAIL_CODE, Const.FAIL_UPDATE_MSG);
		}
	}
}
