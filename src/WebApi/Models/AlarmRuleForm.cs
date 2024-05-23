using MongoDB.Bson;

namespace WebApi.Models
{
    public class AlarmRuleForm
    {
        public string Title { get; set; }

        public string Field { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }

        //public string AlarmConfigId { get; set; }
    }
}

