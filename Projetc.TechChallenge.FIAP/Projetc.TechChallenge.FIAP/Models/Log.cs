namespace Projetc.TechChallenge.FIAP.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public int? ContactId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }

        public Contact Contact { get; set; }
    }
}
