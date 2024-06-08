namespace ExchangeStuff.Service.Models.PermissionGroups
{
    public class PermissionGroupCreateValueModel
    {
        public string Name { get; set; }
        public List<ResourceRecordPermissionValueModel> ResourceRecords { get; set; }
        public List<Guid>? AccountIds { get; set; }
    }
    public record ResourceRecordPermissionValueModel(Guid ResourceId, int PermissionValue);

}
