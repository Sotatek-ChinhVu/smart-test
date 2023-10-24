namespace EmrCloudApi.Responses.MstItem;

public class UploadImageDrugInfResponse
{
    public UploadImageDrugInfResponse(string imageLink)
    {
        ImageLink = imageLink;
    }

    public string ImageLink { get; private set; }
}
