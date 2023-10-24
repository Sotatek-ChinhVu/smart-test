namespace EmrCloudApi.Responses.Receipt;

public class CheckExistsReceInfResponse
{
    public CheckExistsReceInfResponse(bool isExisted)
    {
        IsExisted = isExisted;
    }

    public bool IsExisted { get; private set; }
}
