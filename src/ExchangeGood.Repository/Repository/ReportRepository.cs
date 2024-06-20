using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Enum.Report;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.DAO;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;

namespace ExchangeGood.Repository.Repository
{
	public class ReportRepository : IReportRepository
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;

		public ReportRepository(IUnitOfWork uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}
		public async Task<ReportDto> AddReport(CreateReportRequest reportRequest)
		{
			var report = _mapper.Map<Report>(reportRequest);
			report.ReportId = 0;
			report.CreatedTime = DateTime.Now;
			report.Status = Status.Processing.Name;
			_uow.ReportDAO.AddReport(report);
			
			await _uow.SaveChangesAsync();
			return _mapper.Map<ReportDto>(report);
		}

		public async Task DeleteReport(int reportId)
		{
			Report existedReport = await _uow.ReportDAO.GetReportByIdAsync(reportId);
			if (existedReport == null)
			{
				throw new ReportNotFoundExceoption(reportId);
			}
			_uow.ReportDAO.RemoveReport(existedReport);

			await _uow.SaveChangesAsync();
		}

		public async Task<PagedList<ReportDto>> GetAllReports(ReportParam reportParam)
		{
			var query = _uow.ReportDAO.GetReports(reportParam.Keyword, reportParam.Orderby);

			var result = await PagedList<ReportDto>.CreateAsync(query.ProjectTo<ReportDto>(_mapper.ConfigurationProvider),
			reportParam.PageNumber, reportParam.PageSize);

			return result;
		}

		public async Task<ReportDto> GetReport(int reportId)
		{
			var report = await _uow.ReportDAO.GetReportByIdAsync(reportId);
			return _mapper.Map<ReportDto>(report);
		}

		public async Task<int> UpdateReport(UpdateReportRequest reportRequest)
		{
			Report existedReport = await _uow.ReportDAO.GetReportByIdAsync(reportRequest.ReportId);
			if (existedReport == null)
			{
				throw new ReportNotFoundExceoption(reportRequest.ReportId);
			}
			_mapper.Map(reportRequest, existedReport);
			_uow.ReportDAO.UpdateReport(existedReport);

			await _uow.SaveChangesAsync();
			return existedReport.ReportId;
		}
	}
}
