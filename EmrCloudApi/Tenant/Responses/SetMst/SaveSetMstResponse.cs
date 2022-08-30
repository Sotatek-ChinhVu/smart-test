using Domain.Models.SetMst;

namespace EmrCloudApi.Tenant.Responses.SetMst;

public class SaveSetMstResponse
{
    public SaveSetMstResponse(SetMstModel? setMstModel)
    {
        this.setMstModel = setMstModel;
    }

    public SetMstModel? setMstModel { get; private set; }
}
