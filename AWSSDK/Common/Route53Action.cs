using Amazon.Route53;
using Amazon.Route53.Model;
using AWSSDK.Constants;

namespace AWSSDK.Common
{
    public class Route53Action
    {
        public static async Task<ChangeResourceRecordSetsResponse> CreateTenantDomain(string tenantId)
        {
            try
            {
                var route53Client = new AmazonRoute53Client();

                var subDomain = $"{tenantId}.{ConfigConstant.Domain}";
                var changeBatch = new ChangeBatch
                {
                    Changes = new List<Change>
                {
                    new Change
                    {
                        Action = ChangeAction.CREATE,
                        ResourceRecordSet = new ResourceRecordSet
                        {
                            Name = subDomain,
                            ResourceRecords = new List<ResourceRecord>
                            {
                                new ResourceRecord
                                {
                                    Value = "d1x8o8ft7xbpco.cloudfront.net",
                                },
                            },
                            TTL = 60,
                            Type = RRType.CNAME,
                        },
                    },
                },
                    Comment = "tenan03",
                };

                var response = await route53Client.ChangeResourceRecordSetsAsync(new ChangeResourceRecordSetsRequest
                {
                    ChangeBatch = changeBatch,
                    HostedZoneId = ConfigConstant.HostedZoneId,
                });

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> CheckSubdomainExistence(string subdomainToCheck)
        {
            using (var route53Client = new AmazonRoute53Client())
            {
                var listResourceRecordSetsRequest = new ListResourceRecordSetsRequest
                {
                    HostedZoneId = ConfigConstant.HostedZoneId
                };

                var listResourceRecordSetsResponse = await route53Client.ListResourceRecordSetsAsync(listResourceRecordSetsRequest);

                bool subdomainExists = listResourceRecordSetsResponse.ResourceRecordSets
                    .Any(recordSet => recordSet.Name == $"{subdomainToCheck}.{ConfigConstant.Domain}.");

                if (subdomainExists)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
