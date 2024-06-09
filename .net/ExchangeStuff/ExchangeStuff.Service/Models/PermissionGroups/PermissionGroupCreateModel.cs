namespace ExchangeStuff.Service.Models.PermissionGroups
{
    public class PermissionGroupCreateModel
    {
        public string Name { get; set; }
        public List<ResourceRecord> ResourceRecords { get; set; }
        public List<Guid>? AccountIds { get; set; }
    }
    public record ResourceRecord(Guid ResourceId, List<Guid> ActionIds);

}
