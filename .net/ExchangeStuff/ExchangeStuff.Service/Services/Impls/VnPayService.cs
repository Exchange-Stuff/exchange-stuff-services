using ExchangeStuff.Service.Library;
using ExchangeStuff.Service.Services.Interfaces;
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

        public VNPayService(IConfiguration configuration, VnPayLibrary vnPayLibrary)
        {
            _configuration = configuration;
            _vnPayLibrary = vnPayLibrary;
        }

        public string CreatePaymentUrl()
        {
            var vnp_TmnCode = _configuration["VNPay:vnp_TmnCode"];
            var vnp_HashSecret = _configuration["VNPay:vnp_HashSecret"];
            var vnp_Url = _configuration["VNPay:vnp_Url"];
            var vnp_ReturnUrl = _configuration["VNPay:vnp_ReturnUrl"];
            var amount = 1806000; 

            
            _vnPayLibrary.AddRequestData("vnp_Version", "2.1.0");
            _vnPayLibrary.AddRequestData("vnp_Command", "pay");
            _vnPayLibrary.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            _vnPayLibrary.AddRequestData("vnp_Amount", (amount * 100).ToString()); 
            _vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
            _vnPayLibrary.AddRequestData("vnp_TxnRef", "5");
            _vnPayLibrary.AddRequestData("vnp_OrderInfo", "Thanh toan don hang :5");
            _vnPayLibrary.AddRequestData("vnp_OrderType", "other");
            _vnPayLibrary.AddRequestData("vnp_Locale", "vn");
            _vnPayLibrary.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            _vnPayLibrary.AddRequestData("vnp_IpAddr", "127.0.0.1");
            _vnPayLibrary.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            // Tạo URL thanh toán
            string paymentUrl = _vnPayLibrary.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }
    }
}
