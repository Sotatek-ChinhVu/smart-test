namespace AWSSDK.Interfaces
{
    public interface IAwsSdkService
    {
        Task<Dictionary<string, Dictionary<string, string>>> SummaryCard();
        Task<List<string>> GetAvailableIdentifiersAsync();
        Task<Dictionary<string, string>> TenantOnboardAsync(string tenantId, int size, int sizeType, int tier);
        Task<string> GetInfTenantByTenant(string Id);
    }
}
