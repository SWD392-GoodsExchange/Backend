using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.DAO
{
	public class ReportDAO
	{
		private readonly GoodsExchangeContext _context;

		public ReportDAO(GoodsExchangeContext context)
		{
			_context = context;
		}

		public void AddReport(Report report)
		{
			_context.Reports.Add(report);
		}

		public async Task<Report> GetReportByIdAsync(int id)
		{
			return await _context.Reports.FindAsync(id);
		}

		public IQueryable<Report> GetReports(string keyword, string orderBy)
		{
			var query = _context.Reports
				.Include(p => p.Fe)
				.Include(p => p.Product)
				.AsQueryable();

			// Add another logic later
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(x => x.Message.ToLower().Contains(keyword.ToLower().Trim()));
			}

			query = orderBy switch
			{
				"created" => query.OrderByDescending(u => u.CreatedTime),
				_ => query.OrderBy(u => u.ReportId) // Default ordering, change as needed
			};

			// Add thu tu cua list later

			return query.AsNoTracking();
		}
        public IQueryable<Report> GetReportsProcessing(string keyword, string orderBy)
        {
            var query = _context.Reports
                .Include(p => p.Fe)
                .Include(p => p.Product)
                .AsQueryable();
            query = query.Where(x => x.Status.Equals("Processing"));

            // Add another logic later
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Message.ToLower().Contains(keyword.ToLower().Trim()));
            }

            query = orderBy switch
            {
                "created" => query.OrderByDescending(u => u.CreatedTime),
                _ => query.OrderBy(u => u.ReportId) // Default ordering, change as needed
            };

            // Add thu tu cua list later

            return query.AsNoTracking();
        }

        public IQueryable<Report> GetReportsApproved(string keyword, string orderBy)
        {
			var query = _context.Reports
				.Include(p => p.Fe)
				.Include(p => p.Product)
                .AsQueryable();
            query = query.Where(x => x.Status.Equals("Approved"));

            // Add another logic later
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Message.ToLower().Contains(keyword.ToLower().Trim()));
            }

            query = orderBy switch
            {
                "created" => query.OrderByDescending(u => u.CreatedTime),
                _ => query.OrderBy(u => u.ReportId) // Default ordering, change as needed
            };

            // Add thu tu cua list later

            return query.AsNoTracking();
        }
        public IQueryable<Report> GetReportsRejected(string keyword, string orderBy)
        {
            var query = _context.Reports
                .Include(p => p.Fe)
                .Include(p => p.Product)
                .AsQueryable();
            query = query.Where(x => x.Status.Equals("Rejected"));

            // Add another logic later
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Message.ToLower().Contains(keyword.ToLower().Trim()));
            }

            query = orderBy switch
            {
                "created" => query.OrderByDescending(u => u.CreatedTime),
                _ => query.OrderBy(u => u.ReportId) // Default ordering, change as needed
            };

            // Add thu tu cua list later

            return query.AsNoTracking();
        }

        public IQueryable<Report> GetReportsByProduct(int Id, string keyword, string orderBy)
        {
            var query = _context.Reports
                .Include(p => p.Fe)
                .Include(p => p.Product)
                .AsQueryable();
            query = query.Where(x => x.ProductId.Equals(Id));

            // Add another logic later
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Message.ToLower().Contains(keyword.ToLower().Trim()));
            }

            query = orderBy switch
            {
                "created" => query.OrderByDescending(u => u.CreatedTime),
                _ => query.OrderBy(u => u.ReportId) // Default ordering, change as needed
            };

            // Add thu tu cua list later

            return query.AsNoTracking();
        }

        public void RemoveReport(Report report)
		{
			_context.Reports.Remove(report);
		}

		public void UpdateReport(Report report)
		{
			_context.Reports.Update(report);
		}
	}
}
