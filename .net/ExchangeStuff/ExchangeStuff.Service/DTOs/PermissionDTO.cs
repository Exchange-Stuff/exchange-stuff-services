namespace ExchangeStuff.Service.DTOs
{
    public class PermissionDTO
    {
        public Guid PermissionGroupId { get; set; }
        public Guid ResourceId { get; set; }
        public int PermissionValue { get; set; }
        public PermissionGroupDTO PermissionGroup { get; set; }
        public ResourceDTO Resource { get; set; }
    }
}
