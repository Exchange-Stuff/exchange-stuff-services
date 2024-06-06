using ExchangeStuff.AuthModels;
using ExchangeStuff.Responses;
using ExchangeStuff.Service.DTOs;
using ExchangeStuff.Service.Services.Impls;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;

namespace ExchangeStuff.AuthOptions
{
    public class ActionRequirementHandler : AuthorizationHandler<ActionRequirement>
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ActionRequirementHandler(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionRequirement requirement)
        {
            bool isPassed = false;
            try
            {
                if (!context.User.Identity!.IsAuthenticated || requirement == null! || string.IsNullOrEmpty(requirement.Actions) || context.HasSucceeded)
                {
                    context.Fail();
                    return;
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                    var _permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
                    var _cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                    var _actionService = scope.ServiceProvider.GetRequiredService<IActionService>();

                    var claim = await _tokenService.GetClaimDTOByAccessToken();
                    if (claim == null)
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }
                    var permissiongr = await _cacheService.GetPermissionGroupByAccountId(claim.Id);

                    if (permissiongr == null! || permissiongr.Count == 0)
                    {
                        context.Fail();
                        await ToErrorResult("Forbidden", 403);
                        return;
                    }

                    var permissions = await _permissionService.GetPermissionsCache(permissiongr.Select(x => x.Id).ToList());
                    if (permissions?.Any() != true)
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }

                    var resource = _httpAccessor.HttpContext?.Request.RouteValues["controller"] + "";

                    if (string.IsNullOrEmpty(resource))
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }
                    var permissonActual = permissions.Where(x => x.Resource.Name.ToLower() == resource.ToLower()).FirstOrDefault();

                    if (permissonActual == null!)
                    {
                        context.Fail();
                        await ToErrorResult("Unauthorize", 401);
                        return;
                    }
                    if (await ValidActionResource(_actionService, requirement.Actions, permissonActual.PermissionValue))
                    {
                        context.Succeed(requirement);
                        isPassed = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async Task ToErrorResult(string msgError, int statusCode)
        {
            if (_httpAccessor.HttpContext!.Response.HasStarted)
            {
                ResponseResult<string> response = new ResponseResult<string>
                {
                    Error = new ErrorViewModel
                    {
                        Code = statusCode,
                        Message = msgError
                    },
                    IsSuccess = false,
                    Value = null!
                };

                _httpAccessor.HttpContext.Response.ContentType = "application/json";
                _httpAccessor.HttpContext.Response.StatusCode = statusCode;
                await _httpAccessor.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }

        private async Task<bool> ValidActionResource(IActionService _actionService, string action, int roleValue)
        {
            List<ActionDTO> actionDTOs = await _actionService.GetActionDTOsCache();

            if (actionDTOs?.Any() != true) return false;

            actionDTOs = actionDTOs.OrderBy(x => x.Index).ToList();

            // expected Permission 
            char[] authorizeString = ReverseString(DecimalToBinary(roleValue)).ToArray();

            Dictionary<string, bool> actionKey = new Dictionary<string, bool>();
            int i = 0;
            foreach (var item in authorizeString)
            {
                bool vlue;
                if (item == '0')
                {
                    vlue = false;
                }
                else if (item == '1')
                {
                    vlue = true;
                }
                else
                {
                    return false;
                }
                actionKey.Add(actionDTOs[i].Name.ToLower(), vlue);
                i++;
            }
            if (actionKey.ContainsKey(action.ToLower()))
            {
                return actionKey[action.ToLower()];
            }
            return false;
        }

        private string DecimalToBinary(int decimalNumber)
        {
            if (decimalNumber == 0)
                return "0";

            StringBuilder binary = new StringBuilder();

            while (decimalNumber > 0)
            {
                binary.Insert(0, decimalNumber % 2);
                decimalNumber /= 2;
            }

            return binary.ToString();
        }

        private char[] ReverseString(string str)
        {
            char[] chars = str.ToCharArray();
            int length = str.Length;
            for (int i = 0; i < length / 2; i++)
            {
                char temp = chars[i];
                chars[i] = chars[length - 1 - i];
                chars[length - 1 - i] = temp;
            }
            return chars;
        }
    }
}
