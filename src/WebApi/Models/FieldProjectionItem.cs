namespace WebApi.Models
{
    public class FieldProjectionItem
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Key { get; set; }

        public string FromKey { get; set; }

        public Dictionary<string, string> Mappings { get; set; }

        public string MappingText { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
