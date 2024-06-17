using ExchangeStuff.Core.Entities;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Library;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Impls
{
    public class VNPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly VnPayLibrary _vnPayLibrary;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityUser<Guid> _identityUser;

        public VNPayService(IConfiguration configuration, VnPayLibrary vnPayLibrary, IHttpContextAccessor httpContextAccessor, IIdentityUser<Guid> identityUser)
        {
            _configuration = configuration;
            _vnPayLibrary = vnPayLibrary;
            _httpContextAccessor = httpContextAccessor;
            _identityUser = identityUser;
        }

        public string CreatePaymentUrl()
        {
            var vnp_TmnCode = _configuration["VNPay:vnp_TmnCode"];
            var vnp_HashSecret = _configuration["VNPay:vnp_HashSecret"];
            var vnp_Url = _configuration["VNPay:vnp_Url"];
            var vnp_ReturnUrl = _configuration["VNPay:vnp_ReturnUrl"];
            var amount = 200;
            var total = amount * 100000;
            var random = new Random();
            var txnRef = random.Next(1, 100000).ToString();
            var clientIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            vnp_ReturnUrl = $"{vnp_ReturnUrl}?userId={_identityUser.AccountId}&amount={amount}";

            _vnPayLibrary.AddRequestData("vnp_Version", "2.1.0");
            _vnPayLibrary.AddRequestData("vnp_Command", "pay");
            _vnPayLibrary.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            _vnPayLibrary.AddRequestData("vnp_Amount", total.ToString());
            _vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
            _vnPayLibrary.AddRequestData("vnp_TxnRef", txnRef);
            _vnPayLibrary.AddRequestData("vnp_OrderInfo", "Thanh toan don hang: " + txnRef);
            _vnPayLibrary.AddRequestData("vnp_OrderType", "other");
            _vnPayLibrary.AddRequestData("vnp_Locale", "vn");
            _vnPayLibrary.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            _vnPayLibrary.AddRequestData("vnp_IpAddr", clientIp);
            _vnPayLibrary.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            string paymentUrl = _vnPayLibrary.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }
    }
}
