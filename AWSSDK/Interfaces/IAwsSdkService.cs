namespace AWSSDK.Interfaces
{
    public interface IAwsSdkService
    {
        Task<Dictionary<string, Dictionary<string, string>>> SummaryCard();
        Task<List<string>> GetAvailableIdentifiersAsync();
        Task<string> GetInfTenantByTenant(string Id);
        Task<bool> CheckSubdomainExistenceAsync(string subdomainToCheck);
        Task<bool> IsDedicatedTypeAsync(string dbIdentifier);
    }
}
