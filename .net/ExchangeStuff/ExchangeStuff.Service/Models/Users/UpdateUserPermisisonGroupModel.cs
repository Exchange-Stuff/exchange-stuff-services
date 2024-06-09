namespace ExchangeStuff.Service.Models.Users
{
    public class UpdateUserPermisisonGroupModel
    {
        public Guid AccountId { get; set; }
        public List<Guid>? PermissionGroupIds { get; set; }
    }
}
