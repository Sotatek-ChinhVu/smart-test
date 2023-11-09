namespace EmrCloudApi.Responses.DrugInfor;

public class SaveSinrekiFilterMstListResponse
{
    public SaveSinrekiFilterMstListResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
