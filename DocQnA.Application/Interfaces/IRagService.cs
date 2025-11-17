namespace DocQnA.Application.Interfaces
{
    public interface IRagService
    {
        Task<RagResult> QueryAsync(string query);
    }

}