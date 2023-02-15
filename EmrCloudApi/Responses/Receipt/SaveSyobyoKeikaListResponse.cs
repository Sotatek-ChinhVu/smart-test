namespace EmrCloudApi.Responses.Receipt;

public class SaveSyobyoKeikaListResponse
{
    public SaveSyobyoKeikaListResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
