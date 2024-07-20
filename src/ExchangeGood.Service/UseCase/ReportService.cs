using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Enum.Member;
using ExchangeGood.Contract.Payloads.Request.Report;
using ExchangeGood.Contract.Payloads.Response;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Service.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeGood.Contract.Payloads.Request.Product;
using Microsoft.Extensions.Options;

namespace ExchangeGood.Service.UseCase
{
    public class ReportService : IReportService
	{
		private readonly IProductRepository _productRepository;
		private readonly IReportRepository _reportRepository;
		private readonly IMemberRepository _memberRepository;
        private readonly SmtpSettings _smtpsetting;
        private readonly IMemberService _memberservice;

        public ReportService(IProductRepository productRepository, IReportRepository reportRepository, IMemberRepository memberRepository, IOptions<SmtpSettings> smtpSetting, IMemberService memberService)
		{
			_productRepository = productRepository;
			_reportRepository = reportRepository;
			_memberRepository = memberRepository;
            _smtpsetting = smtpSetting.Value;
            _memberservice = memberService;
        }

        public async Task<ReportDto> AddReport(CreateReportRequest reportRequest)
        {
            var existingProduct = await _productRepository.GetProduct(reportRequest.ProductId);
            var existingMember = await _memberRepository.GetMemberById(reportRequest.FeId);
            if(existingProduct != null && existingMember != null) {
                var report = await _reportRepository.AddReport(reportRequest);
                await SendReportAddedEmail(reportRequest.FeId, existingProduct.Title);

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

        public async Task<int> GetTotalReportsAsync()
        {
            return await _reportRepository.GetTotalReportsAsync();
        }

        public async Task<ReportDto> UpdateReportStatusApproved(int reportId)
        {
            var report = await _reportRepository.GetReport(reportId);
            var product = await _productRepository.GetProduct(report.ProductId);
            if (report != null)
            {
                await SendReportUpdatedEmailApproved(report.FeId, product.Title);

                var updatedReport = await _reportRepository.UpdateReportStatusApproved(reportId);

                var products = await _productRepository.GetProductsByFeId(product.FeId);


                //int approvedReportCount = await _reportRepository.CountApprovedReportsForProduct(product.ProductId);

                //if (approvedReportCount >= 3)
                //{
                //    await _memberRepository.UpdateMemberStatus(product.FeId);
                //}

                int totalApprovedReportCount = 0;

                foreach (var prod in products)
                {
                    int approvedReportCount = await _reportRepository.CountApprovedReportsForProduct(prod.ProductId);
                    totalApprovedReportCount += approvedReportCount;
                    if (totalApprovedReportCount >= 3)
                    {
                        await _memberRepository.UpdateMemberStatus(product.FeId);
                        break;
                    }

                }

                return updatedReport;
            }

            return null;
        }

        public async Task<ReportDto> UpdateReportStatusRejected(int reportId)
        {
            var report = await _reportRepository.GetReport(reportId);
            var product = await _productRepository.GetProduct(report.ProductId);

            if (report != null)
            {
                await SendReportUpdatedEmailRejected(report.FeId, product.Title);

                return await _reportRepository.UpdateReportStatusRejected(reportId);
            }
            return null;
        }

        private async Task SendReportAddedEmail(string feId, string productNamme)
        {
            var member = await _memberservice.GetMemberByFeId(feId);
            if (member == null || string.IsNullOrEmpty(member.Email))
            {
                throw new Exception($"Member with FeId {feId} not found or has no email specified.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ExchangeGood System", _smtpsetting.Username));
            message.To.Add(new MailboxAddress(member.UserName, member.Email));
            message.Subject = "New Report sended";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
            <p>Dear {member.UserName},</p>
            <p>We are pleased to inform you that your report to Product: <strong>{productNamme}</strong> has been successfully sended to ExchangeGood.</p>
            <p>Please, wait! Your report will be reviewed soon.</p>
            <p>Thank you for using our platform!</p>
            <p>Best regards,</p>
            <p>The ExchangeGood Team</p>
        ";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_smtpsetting.SmtpServer, _smtpsetting.Port, _smtpsetting.UseSsl);
                    client.Authenticate(_smtpsetting.Username, _smtpsetting.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

        private async Task SendReportUpdatedEmailApproved(string feId, string productNamme)
        {
            var member = await _memberservice.GetMemberByFeId(feId);
            if (member == null || string.IsNullOrEmpty(member.Email))
            {
                throw new Exception($"Member with FeId {feId} not found or has no email specified.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ExchangeGood System", _smtpsetting.Username));
            message.To.Add(new MailboxAddress(member.UserName, member.Email));
            message.Subject = "New Report sended";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
            <p>Dear {member.UserName},</p>
            <p>We are pleased to inform you that your report to Product: <strong>{productNamme}</strong> has been successfully approved to ExchangeGood.</p>
            <p>If you have any feedbacks, please respond to this mail!</p>
            <p>Thank you for using our platform!</p>
            <p>Best regards,</p>
            <p>The ExchangeGood Team</p>
        ";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_smtpsetting.SmtpServer, _smtpsetting.Port, _smtpsetting.UseSsl);
                    client.Authenticate(_smtpsetting.Username, _smtpsetting.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }


        private async Task SendReportUpdatedEmailRejected(string feId, string productNamme)
        {
            var member = await _memberservice.GetMemberByFeId(feId);
            if (member == null || string.IsNullOrEmpty(member.Email))
            {
                throw new Exception($"Member with FeId {feId} not found or has no email specified.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ExchangeGood System", _smtpsetting.Username));
            message.To.Add(new MailboxAddress(member.UserName, member.Email));
            message.Subject = "New Report sended";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
            <p>Dear {member.UserName},</p>
            <p>We are pleased to inform you that your report to Product: <strong>{productNamme}</strong> has been successfully rejected to ExchangeGood.</p>
            <p>If you have any feedbacks, please respond to this mail!</p>
            <p>Thank you for using our platform!</p>
            <p>Best regards,</p>
            <p>The ExchangeGood Team</p>
        ";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_smtpsetting.SmtpServer, _smtpsetting.Port, _smtpsetting.UseSsl);
                    client.Authenticate(_smtpsetting.Username, _smtpsetting.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

    }
}
