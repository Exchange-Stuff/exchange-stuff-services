using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Models.Accounts;
using ExchangeStuff.Service.Models.Actions;
using ExchangeStuff.Service.Models.Admins;
using ExchangeStuff.Service.Models.PermissionGroups;
using ExchangeStuff.Service.Models.Permissions;
using ExchangeStuff.Service.Models.Resources;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    /// <summary>
    /// Only admin call this controller, TODO: I'll auth later
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly IAdminService _adminService;

        /// <summary>
        /// Admin behavior
        /// </summary>
        /// <param name="adminService"></param>
        public AdminController(IAdminService adminService, ICacheService cacheService)
        {
            _cacheService = cacheService;
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

        [HttpPost("action")]
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
        [HttpPost("resource")]
        public async Task<IActionResult> CreateResource([FromBody] string name)
        {
            var rs = await _adminService.CreateResource(name);
            if (!rs) throw new Exception("Can't create resource, CreateResource");

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

        [ESAuthorize(new string[] {
        ActionConstant.READ
        })]
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

        /// <summary>
        /// 1 session 1 devide
        /// </summary>
        /// <param name="adminLoginRd"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginRd adminLoginRd)
        {
            return Ok(new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _adminService.LoginAdmin(adminLoginRd.username, adminLoginRd.password)
            });
        }

        /// <summary>
        /// Provice token with request
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = (HttpContext.Request.Headers.Authorization.FirstOrDefault() + "").Split(" ").Last();
            if (string.IsNullOrEmpty(token)) throw new Exception("No token in header");
            return Ok(new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = (await _cacheService.DeleteAccessToken(token)) + ""
            });
        }

        public sealed record AdminLoginRd(string username, string password);

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAdmin(AdminCreateModel adminCreateModel)
        {
            return Ok(new ResponseResult<AdminViewModel>
            {
                Error = null!,
                IsSuccess = true,
                Value = await _adminService.CreateAdmin(adminCreateModel.Username, adminCreateModel.Password, adminCreateModel.Name)
            });
        }

        [HttpPut("permissionGroup/permissions")]

        public async Task<IActionResult> UpdatePermissionGroup([FromBody] UpdatePermissionActionValueModel updatePermissionActionValueModel)
        {
            var rs = await _adminService.AddPermissionIntoPermissionGroup(updatePermissionActionValueModel);
            if (!rs) throw new Exception("Can't update permission group with action value, UpdatePermissionGroup");

            return StatusCode(StatusCodes.Status200OK, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs.ToString()
            });
        }

        [HttpPost("create/account")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreateModel accountCreateModel)
        {
            var rs = await _adminService.CreateAccount(accountCreateModel);
            return rs ? StatusCode(StatusCodes.Status201Created, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs + ""
            }) : throw new Exception("Create account fail, something wrong");
        }

        [HttpDelete("permissionGroup/{id}")]
        public async Task<IActionResult> DeletePermissionGroup([FromRoute] Guid id)
        {
            var rs = await _adminService.DeletePermissionGroup(id);
            return rs ? StatusCode(StatusCodes.Status204NoContent, new ResponseResult<string>
            {
                Error = null!,
                IsSuccess = true,
                Value = rs + ""
            }) : throw new Exception("Delete permission group fail");
        }
    }
}
