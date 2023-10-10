namespace EmrCloudApi.Responses.MstItem;

public class SaveRenkeiResponse
{
    public SaveRenkeiResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
