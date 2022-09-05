namespace Infrastructure.Options;

public class AmazonS3Options
{
    public const string Position = "AmazonS3";

    public string AwsAccessKeyId { get; set; } = string.Empty;
    public string AwsSecretAccessKey { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string BaseAccessUrl { get; set; } = string.Empty;
}
