using Domain.Models.Diseases;
using Domain.Models.ListSetMst;

namespace EmrCloudApi.Requests.Diseases;

public class UpdateByomeiSetMstRequest
{
    public List<ByomeiSetMstUpdateModel> ByomeiSetMsts { get; set; } = new();
}
