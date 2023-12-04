using Amazon.CloudFront;
using Amazon.CloudFront.Model;
using AWSSDK.Constants;


namespace AWSSDK.Common
{
    public class CloudFrontAction
    {
        public static async Task<UpdateDistributionResponse> UpdateAlterCNAMEAsync(string? eTag, List<string> cname)
        {
            try
            {
                var cloudFrontClient = new AmazonCloudFrontClient();
                var request = new UpdateDistributionRequest
                {
                    DistributionConfig = new DistributionConfig
                    {
                        CallerReference = "4531dda9-e0db-4080-83ef-04d572e4407f",
                        Aliases = new Aliases
                        {
                            Quantity = cname.Count(),
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
                            CachePolicyId = ConfigConstant.ManagedCachingOptimized,
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
                            ACMCertificateArn = "arn:aws:acm:us-east-1:519870134487:certificate/523b4cb6-2f4e-4da3-a44f-e1922d30b00c",
                            SSLSupportMethod = "sni-only",
                            MinimumProtocolVersion = "TLSv1.2_2021",
                            Certificate = "arn:aws:acm:us-east-1:519870134487:certificate/523b4cb6-2f4e-4da3-a44f-e1922d30b00c",
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
                    Id = ConfigConstant.DistributionId
                };

                return await cloudFrontClient.UpdateDistributionAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"UpdateAlterCNAME. {ex.Message}");
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
                response.DistributionConfig.DefaultCacheBehavior.CachePolicyId = "Managed-CachingOptimized";
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
                var distInfo = await GetDistributionConfigAsync(ConfigConstant.DistributionId);

                if (distInfo != null)
                {
                    var eTag = distInfo["ETag"].ToString();
                    var quantity = (int)distInfo["Quantity"];
                    var alterName = (List<string>)distInfo["AlterName"];
                    alterName.Add($"{tenantId}.{ConfigConstant.Domain}");

                    await UpdateAlterCNAMEAsync(eTag, alterName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"UpdateAlterCNAME. {ex.Message}");
            }
        }

        public static async Task<bool> RemoveItemCnameAsync(string tenantId)
        {
            try
            {
                var distInfo = await GetDistributionConfigAsync(ConfigConstant.DistributionId);

                if (distInfo != null)
                {
                    var eTag = distInfo["ETag"].ToString();
                    var quantity = (int)distInfo["Quantity"];
                    var alterName = (List<string>)distInfo["AlterName"];
                    alterName.Remove($"{tenantId}.{ConfigConstant.Domain}");

                    // Update the distribution with the modified CNAME list
                    UpdateDistributionResponse resUpdate = await UpdateAlterCNAMEAsync(eTag, alterName);

                    // Check if the update was successful
                    if (resUpdate != null && resUpdate.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine("CNAME removed successfully!");
                        return true;
                    }
                    else
                    {
                        throw new Exception($"Remove Item Cname. Code: {resUpdate?.HttpStatusCode}");
                    }
                }
                else
                {
                    throw new Exception($"Remove Item Cname. Distribution info empty");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Remove Item Cname. {ex.Message}");
            }
        }
    }
}
