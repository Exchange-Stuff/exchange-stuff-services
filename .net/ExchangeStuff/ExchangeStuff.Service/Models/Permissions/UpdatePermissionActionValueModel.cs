namespace ExchangeStuff.Service.Models.Permissions
{
    public class UpdatePermissionActionValueModel
    {
        public Guid PermissionGroupId { get; set; }
        public List<ResourceValueRecord> ResourceValueRecords { get; set; }
    }

    public sealed record ResourceValueRecord(Guid ResourceId, int PermissionValue);
}
