namespace EmrCloudApi.Responses.AccountDue;

public class IsNyukinExistedResponse
{
    public IsNyukinExistedResponse(bool isExisted)
    {
        IsExisted = isExisted;
    }

    public bool IsExisted { get; private set; }
}
