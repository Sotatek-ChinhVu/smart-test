using Amazon.S3;
using Domain.Common;

namespace Domain.Models.ReleasenoteRead;

public interface IReleasenoteReadRepository : IRepositoryBase
{
    Task<List<ReleasenoteReadModel>> GetLoadListVersion(int hpId, int userId, AmazonS3Client sourceClient, string sourceBucketName);
}
