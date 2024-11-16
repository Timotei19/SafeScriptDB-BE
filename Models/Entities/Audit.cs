namespace Models.Entities
{
    public class Audit
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string DatabaseName { get; set; }

        public int StatusId { get; set; }

        //public int Result { get; set; }

        public bool RollbackDone { get; set; }

        public int UserId { get; set; }

        public List<AuditItem> AuditItems { get; set; }
    }
}
