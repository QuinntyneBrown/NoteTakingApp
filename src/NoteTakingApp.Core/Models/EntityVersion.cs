namespace NoteTakingApp.Core.Models
{
    public class EntityVersion
    {
        public int EntityVersionId { get; set; }
        public int EntityId { get; set; }
        public int Version { get; set; }
        public string EntityName { get; set; }
    }
}
