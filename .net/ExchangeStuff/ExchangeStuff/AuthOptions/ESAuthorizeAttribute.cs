using Microsoft.AspNetCore.Authorization;

namespace ExchangeStuff.AuthOptions
{
    public class ESAuthorizeAttribute : AuthorizeAttribute
    {
        public const string ActionGroup = "Actions";
        public string[] Actions { get; set; }

        public ESAuthorizeAttribute(string[] actions)
        {
            Actions = actions;
            Policy += $"{ActionGroup}${string.Join("|", Actions)};";
        }
    }
}
