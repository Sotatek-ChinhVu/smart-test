using Domain.Models.SetMst;
using UseCase.SetMst.GetList;

namespace EmrCloudApi.Responses.SetMst;

public class SaveSetMstResponse
{
    public SaveSetMstResponse(List<GetSetMstListOutputItem> data)
    {
        Data = data;
    }

    public List<GetSetMstListOutputItem> Data { get; private set; }
}
