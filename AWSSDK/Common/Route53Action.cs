using Amazon.Route53;
using Amazon.Route53.Model;

namespace AWSSDK.Common
{
    public class Route53Action
    {
        public static async Task<ChangeResourceRecordSetsResponse> CreateTenantDomain(string tenantId)
        {
            try
            {
                var route53Client = new AmazonRoute53Client();

                var subDomain = $"{tenantId}.smartkarte.org";
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

                var hostedZoneId = "Z09462362PXK5JFYQ59B";

                var response = await route53Client.ChangeResourceRecordSetsAsync(new ChangeResourceRecordSetsRequest
                {
                    ChangeBatch = changeBatch,
                    HostedZoneId = hostedZoneId,
                });

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
