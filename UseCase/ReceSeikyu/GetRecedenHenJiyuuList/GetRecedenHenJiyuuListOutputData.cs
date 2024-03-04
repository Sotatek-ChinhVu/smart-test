using Domain.Models.ReceSeikyu;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.GetRecedenHenJiyuuList;

public class GetRecedenHenJiyuuListOutputData : IOutputData
{
    public GetRecedenHenJiyuuListOutputData(GetRecedenHenJiyuuListStatus status, List<RecedenHenJiyuuModel> recedenHenJiyuuModelList)
    {
        Status = status;
        RecedenHenJiyuuModelList = recedenHenJiyuuModelList;
    }

    public GetRecedenHenJiyuuListStatus Status { get; private set; }

    public List<RecedenHenJiyuuModel> RecedenHenJiyuuModelList { get; private set; }
}
