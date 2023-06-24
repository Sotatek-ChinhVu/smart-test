using Domain.Models.SetMst;

namespace EmrCloudApi.Responses.SetMst;

public class SaveSetMstResponse
{
    public SaveSetMstResponse(List<SetMstModel> setMstModel)
    {
        SetMstModel = setMstModel;
    }

    public List<SetMstModel> SetMstModel { get; private set; }
}
