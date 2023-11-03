using Domain.Models.ListSetMst;

namespace EmrCloudApi.Requests.ListSetMst;

public class UpdateListSetMstRequest
{
    public List<ListSetMstUpdateModel> ListSetMsts { get; set; } = new();
}
