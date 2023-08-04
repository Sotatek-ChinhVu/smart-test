namespace EmrCloudApi.Responses.MstItem;

public class UploadImageDrugInfResponse
{
    public UploadImageDrugInfResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
