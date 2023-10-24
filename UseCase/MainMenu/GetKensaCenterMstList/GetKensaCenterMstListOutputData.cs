using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaCenterMstList;

public class GetKensaCenterMstListOutputData:IOutputData
{
    public GetKensaCenterMstListOutputData(List<KensaCenterMstModel> kensaCenterMstList, GetKensaCenterMstListStatus status)
    {
        KensaCenterMstList = kensaCenterMstList;
        Status = status;
    }

    public List<KensaCenterMstModel> KensaCenterMstList { get;private set; }

    public GetKensaCenterMstListStatus Status { get;private set; }
}
