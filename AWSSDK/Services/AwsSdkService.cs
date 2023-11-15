using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using AWSSDK.Common;
using AWSSDK.Interfaces;

namespace AWSSDK.Services
{
    public class AwsSdkService : IAwsSdkService
    {
        public AwsSdkService() { }
        public async Task<Dictionary<string, Dictionary<string, string>>> SummaryCard()
        {
            return await CloudWatchAction.GetSummaryCardAsync();
        }
        public async Task<List<string>> GetAvailableIdentifiersAsync()
        {
            var sumaryCard = await CloudWatchAction.GetSummaryCardAsync();
            var result = sumaryCard.Where(entry => entry.Value["available"] == "yes").Select(entry => entry.Key).ToList();
            return result;
        }

        public  async Task<string> GetInfTenantByTenant(string id)
        {
            return "";
        }
    }
}
