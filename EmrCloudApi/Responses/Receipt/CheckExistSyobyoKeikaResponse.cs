namespace EmrCloudApi.Responses.Receipt;

public class CheckExistSyobyoKeikaResponse
{
    public CheckExistSyobyoKeikaResponse(bool isExisted)
    {
        IsExisted = isExisted;
    }

    public bool IsExisted { get; private set; }
}
