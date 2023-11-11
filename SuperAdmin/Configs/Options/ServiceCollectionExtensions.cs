using Infrastructure.Options;

namespace EmrCloudApi.Configs.Options;

public static class ServiceCollectionExtensions
{
    public static void AddEmrOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AmazonS3Options>(configuration.GetSection(AmazonS3Options.Position));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position));
    }
}
