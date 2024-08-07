﻿using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.BanReasons;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanReasonController : ControllerBase
    {
        private readonly IBanReasonService _banReasonService;

        public BanReasonController(IBanReasonService banReasonService)
        {
            _banReasonService = banReasonService;
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBanReason([FromRoute] Guid id)
        => Ok(new ResponseResult<BanReasonViewModel>
        {
            Error = null!,
            IsSuccess = true,
            Value = await _banReasonService.GetBanReason(id)
        });

        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet("reasons")]
        public async Task<IActionResult> GetBanReason(string? content = null, BanReasonType? banReasonType = null!)
        => Ok(new ResponseResult<List<BanReasonViewModel>>
        {
            Error = null!,
            IsSuccess = true,
            Value = await _banReasonService.GetBanReasons(content, banReasonType)
        });
        [ESAuthorize(new string[] { ActionConstant.WRITE })]

        [HttpPost]
        public async Task<IActionResult> CreateBanReason(BanReasonCreateModel banReasonCreateModel)
        {
            var rs = await _banReasonService.CreateBanReasons(banReasonCreateModel);
            return rs != null ? StatusCode(StatusCodes.Status201Created, rs) : throw new Exception("Create ban reason fail");
        }
        [ESAuthorize(new string[] { ActionConstant.OVERWRITE })]

        [HttpPut]
        public async Task<IActionResult> UpdateBanReason(BanReasonUpdateModel banReasonUpdateModel)
        {
            var rs = await _banReasonService.UpdateBanReasons(banReasonUpdateModel);
            return rs != null ? StatusCode(StatusCodes.Status201Created, rs) : throw new Exception("Update ban reason fail");
        }
    }
}
