namespace WebApplication1.Models
{
    public class AuditLogs
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string UserId { get; set; }
        public string RequestPath { get; set; }
        public string RequestMethod { get; set; }
        public string RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public DateTime EventDate { get; set; }
    }
}
