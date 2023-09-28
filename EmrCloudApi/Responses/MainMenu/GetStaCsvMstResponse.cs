using Domain.Models.MainMenu;

namespace EmrCloudApi.Responses.MainMenu;

public class GetStaCsvMstResponse
{
    public GetStaCsvMstResponse(List<StaCsvMstModel> staCsvMstModels)
    {
        StaCsvMstModels = staCsvMstModels;
    }

    public List<StaCsvMstModel> StaCsvMstModels { get; private set; }
}
