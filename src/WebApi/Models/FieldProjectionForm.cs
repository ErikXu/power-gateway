namespace WebApi.Models
{
    public class FieldProjectionForm
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public string FromKey { get; set; }

        public Dictionary<string, string> Mappings { get; set; }
    }
}

