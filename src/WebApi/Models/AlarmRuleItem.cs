namespace WebApi.Models
{
    public class AlarmRuleItem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Field { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }

        public string AlarmConfigId { get; set; }

        public string AlarmConfigText { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
