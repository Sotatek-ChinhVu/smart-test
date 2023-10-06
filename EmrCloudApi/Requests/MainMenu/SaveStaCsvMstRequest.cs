using Domain.Models.MainMenu;

namespace EmrCloudApi.Requests.MainMenu;

public class SaveStaCsvMstRequest
{
    public List<StaCsvMstModel> StaCsvMstModels { get; set; } = new();
}
