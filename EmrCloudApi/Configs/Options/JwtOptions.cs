namespace EmrCloudApi.Configs.Options;

public class JwtOptions
{
    public const string Position = "Jwt";

    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Token lifetime in hours
    /// </summary>
    public double TokenLifetime { get; set; }
}
