using Microsoft.AspNetCore.Authorization;

namespace ExchangeStuff.AuthModels
{
    public class ActionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Param action
        /// </summary>
        public string Actions { get; set; }
        public ActionRequirement(string actions)
        {
            Actions = actions;
        }
    }
}
