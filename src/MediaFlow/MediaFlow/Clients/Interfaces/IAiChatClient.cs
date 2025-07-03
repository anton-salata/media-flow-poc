namespace MediaFlow.Clients.Interfaces
{
    public interface IAiChatClient
    {
        Task<string> GetAnswer(string prompt);
    }
}