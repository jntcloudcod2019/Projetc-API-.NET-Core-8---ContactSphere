namespace Projetc.TechChallenge.FIAP.Interfaces
{
    public interface ILogService
    {
        Task LogAsync(int? contactId, string action, string details);
    }
}
