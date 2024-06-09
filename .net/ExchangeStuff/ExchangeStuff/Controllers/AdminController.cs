using ExchangeStuff.Responses;
using ExchangeStuff.Service.Models.Actions;
using ExchangeStuff.Service.Models.PermissionGroups;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.Resources;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("permissionGroups")]
        public async Task<IActionResult> CreatePermisisonGroup([FromBody] PermissionGroupCreateModel permissionGroupCreateModel)
        {
            var rs = await _adminService.CreatePermissionGroup(permissionGroupCreateModel);
            if (!rs) throw new Exception("Can't create permission group, CreatePermisisonGroup");

            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

        [HttpPost("permissionGroup/value")]
        public async Task<IActionResult> CreatePermissionGroupValue([FromBody] PermissionGroupCreateValueModel permissionGroupCreateValueModel)
        {
            var rs = await _adminService.CreatePermissionGroupValue(permissionGroupCreateValueModel);
            if (!rs) throw new Exception("Can't create permission group value, CreatePermissionGroupValue");

            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

        [HttpPost("actions")]
        public async Task<IActionResult> CreateAction([FromBody] string name)
        {
            var rs = await _adminService.CreateAction(name);
            if (!rs) throw new Exception("Can't create action, CreateAction");

            return StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

        [HttpPut("permissionAction/value")]
        public async Task<IActionResult> UpdatePermissionActionValue([FromBody] UpdatePermissionActionValueModel updatePermissionActionValueModel)
        {
            var rs = await _adminService.UpdatePermissionActionValue(updatePermissionActionValueModel);
            if (!rs) throw new Exception("Can't update Permission with action value, UpdatePermissionActionValue");

            return StatusCode(StatusCodes.Status200OK, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

        [HttpPut("accounts/permissionGroup")]
        public async Task<IActionResult> UpdatePermissionGroupOfAccount([FromBody] UpdateUserPermisisonGroupModel updateUserPermisisonGroupModel)
        {
            var rs = await _adminService.UpdatePermissionGroupOfAccount(updateUserPermisisonGroupModel);
            if (!rs) throw new Exception("Can't update account permission, UpdatePermissionGroupOfAccount");

            return StatusCode(StatusCodes.Status200OK, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions(int? pageIndex = null!, int? pageSize = null!)
        {
            return Ok(new ResponseResult<List<PermissionViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _adminService.GetPermissions(pageIndex, pageSize)
            });
        }

        [HttpGet("permissionGroups")]
        public async Task<IActionResult> GetPermissionGroups(string? name = null!, int? pageIndex = null!, int? pageSize = null!)
        {
            return Ok(new ResponseResult<List<PermisisonGroupViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _adminService.GetPermisisonGroups(name, pageIndex, pageSize)
            });
        }
        [HttpGet("actions")]
        public async Task<IActionResult> GetActions(string? name = null!, int? pageIndex = null!, int? pageSize = null!)
        {
            return Ok(new ResponseResult<List<ActionViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _adminService.GetActions(name, pageIndex, pageSize)
            });
        }
        [HttpGet("resources")]
        public async Task<IActionResult> GetResources(string? name = null!, int? pageIndex = null!, int? pageSize = null!)
        {
            return Ok(new ResponseResult<List<ResourceViewModel>>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _adminService.GetResources(name, pageIndex, pageSize)
            });
        }
    }
}
