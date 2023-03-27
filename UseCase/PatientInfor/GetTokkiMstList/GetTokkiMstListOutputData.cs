using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetTokiMstList;

public class GetTokkiMstListOutputData : IOutputData
{
    public GetTokkiMstListOutputData(List<TokkiMstModel> tokkiMstList, GetTokkiMstListStatus status)
    {
        TokkiMstList = tokkiMstList;
        Status = status;
    }

    public List<TokkiMstModel> TokkiMstList { get; private set; }

    public GetTokkiMstListStatus Status { get; private set; }
}
