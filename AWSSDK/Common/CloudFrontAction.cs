using Amazon.CloudFront;
using Amazon.CloudFront.Model;
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AWSSDK.Common
{
    public class CloudFrontAction
    {
        public static async Task<UpdateDistributionResponse> UpdateAlterCNAMEAsync(string tenantId, string? eTag, int quantity, List<string> alterName)
        {
            try
            {
                var cloudFrontClient = new AmazonCloudFrontClient();
                var cname = new List<string> { $"{tenantId}.smartkarte.org" };
                cname.AddRange(alterName);
                var request = new UpdateDistributionRequest
                {
                    DistributionConfig = new DistributionConfig
                    {
                        CallerReference = "4531dda9-e0db-4080-83ef-04d572e4407f",
                        Aliases = new Aliases
                        {
                            Quantity = quantity + 1,
                            Items = cname
                        },
                        DefaultRootObject = "",
                        Origins = new Origins
                        {
                            Quantity = 1,
                            Items = new List<Origin>
                        {
                            new Origin
                            {
                                Id = "develop-smartkarte-frontend-bucket.s3-website-ap-northeast-1.amazonaws.com",
                                DomainName = "develop-smartkarte-frontend-bucket.s3-website-ap-northeast-1.amazonaws.com",
                                OriginPath = "",
                                CustomHeaders = new CustomHeaders
                                {
                                    Quantity = 0
                                },
                                CustomOriginConfig = new CustomOriginConfig
                                {
                                    HTTPPort = 80,
                                    HTTPSPort = 443,
                                    OriginProtocolPolicy = "http-only",
                                    OriginSslProtocols = new OriginSslProtocols
                                    {
                                        Quantity = 3,
                                        Items = new List<string> { "TLSv1", "TLSv1.1", "TLSv1.2" }
                                    },
                                    OriginReadTimeout = 30,
                                    OriginKeepaliveTimeout = 5
                                },
                                ConnectionAttempts = 3,
                                ConnectionTimeout = 10,
                                OriginShield = new OriginShield
                                {
                                    Enabled = false
                                }
                            }
                        }
                        },
                        DefaultCacheBehavior = new DefaultCacheBehavior
                        {
                            TargetOriginId = "develop-smartkarte-frontend-bucket.s3-website-ap-northeast-1.amazonaws.com",
                            TrustedSigners = new TrustedSigners
                            {
                                Enabled = false,
                                Quantity = 0
                            },
                            TrustedKeyGroups = new TrustedKeyGroups
                            {
                                Enabled = false,
                                Quantity = 0
                            },
                            ViewerProtocolPolicy = "redirect-to-https",
                            AllowedMethods = new AllowedMethods
                            {
                                Quantity = 7,
                                Items = new List<string> { "HEAD", "DELETE", "POST", "GET", "OPTIONS", "PUT", "PATCH" },
                                CachedMethods = new CachedMethods
                                {
                                    Quantity = 2,
                                    Items = new List<string> { "HEAD", "GET" }
                                }
                            },
                            SmoothStreaming = false,
                            Compress = true,
                            LambdaFunctionAssociations = new LambdaFunctionAssociations
                            {
                                Quantity = 0
                            },
                            FunctionAssociations = new FunctionAssociations
                            {
                                Quantity = 0
                            },
                            FieldLevelEncryptionId = "",
                            CachePolicyId = "4135ea2d-6df8-44a3-9df3-4b5a84be39ad",
                            OriginRequestPolicyId = "88a5eaf4-2fd4-4709-b370-b4c650ea3fcf"
                        },
                        CacheBehaviors = new CacheBehaviors
                        {
                            Quantity = 0
                        },
                        CustomErrorResponses = new CustomErrorResponses
                        {
                            Quantity = 0
                        },
                        Comment = "Multitenant FE",
                        Logging = new Amazon.CloudFront.Model.LoggingConfig
                        {
                            Enabled = false,
                            IncludeCookies = false,
                            Bucket = "",
                            Prefix = ""
                        },
                        PriceClass = "PriceClass_All",
                        Enabled = true,
                        ViewerCertificate = new ViewerCertificate
                        {
                            CloudFrontDefaultCertificate = false,
                            ACMCertificateArn = "arn:aws:acm:us-east-1:519870134487:certificate/f3d9a1e8-8b06-4fb6-9d99-2e05c3855d1f",
                            SSLSupportMethod = "sni-only",
                            MinimumProtocolVersion = "TLSv1.2_2021",
                            Certificate = "arn:aws:acm:us-east-1:519870134487:certificate/f3d9a1e8-8b06-4fb6-9d99-2e05c3855d1f",
                            CertificateSource = "acm"
                        },
                        Restrictions = new Restrictions
                        {
                            GeoRestriction = new GeoRestriction
                            {
                                RestrictionType = "none",
                                Quantity = 0
                            }
                        },
                        WebACLId = "",
                        HttpVersion = "http2",
                        IsIPV6Enabled = true
                    },
                    IfMatch = eTag,
                    Id = "E1Q6ZVLBFAFBDX"
                };

                return await cloudFrontClient.UpdateDistributionAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public static async Task<Dictionary<string, object>> GetDistributionConfigAsync(string id)
        {
            try
            {
                var cloudFrontClient = new AmazonCloudFrontClient();

                var response = await cloudFrontClient.GetDistributionConfigAsync(new GetDistributionConfigRequest
                {
                    Id = id
                });

                var distInfo = new Dictionary<string, object>
            {
                { "ETag", response.ETag },
                { "Quantity", response.DistributionConfig.Aliases.Quantity },
                { "AlterName", response.DistributionConfig.Aliases.Items }
            };

                return distInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public static async Task UpdateNewTenantAsync(string tenantId)
        {
            try
            {
                var distInfo = await GetDistributionConfigAsync("E1Q6ZVLBFAFBDX");

                if (distInfo != null)
                {
                    var eTag = distInfo["ETag"].ToString();
                    var quantity = (int)distInfo["Quantity"];
                    var alterName = (List<string>)distInfo["AlterName"];

                    await UpdateAlterCNAMEAsync(tenantId, eTag, quantity, alterName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
