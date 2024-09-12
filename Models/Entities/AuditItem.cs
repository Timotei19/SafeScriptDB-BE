using System.Text.Json.Serialization;

namespace Models.Entities
{
    public class AuditItem
    {
        public int Id { get; set; }

        public string ScriptName { get; set; }

        public int Status { get; set; }

        public int Result { get; set; }

        public string ResultMessage { get; set; }

        public int AuditId { get; set; }

        [JsonIgnore]
        public Audit Audit { get; set; }
    }
}
