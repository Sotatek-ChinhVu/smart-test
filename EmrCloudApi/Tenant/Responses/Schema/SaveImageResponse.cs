namespace EmrCloudApi.Tenant.Responses.Schema;

public class SaveImageResponse
{
    public SaveImageResponse(string urlImage)
    {
        UrlImage = urlImage;
    }

    public string UrlImage { get; private set; }
}
