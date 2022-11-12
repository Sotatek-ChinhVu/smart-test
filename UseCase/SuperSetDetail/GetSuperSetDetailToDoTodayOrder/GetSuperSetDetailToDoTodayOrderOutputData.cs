using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

public class GetSuperSetDetailToDoTodayOrderOutputData : IOutputData
{
    public GetSuperSetDetailToDoTodayOrderOutputData(List<SetByomeiItem> setByomeiItems, List<SetKarteInfModel> setKarteInfItems, List<SetOrderInfItem> setOrderInfItems, GetSuperSetDetailToDoTodayOrderStatus status)
    {
        SetByomeiItems = setByomeiItems;
        SetKarteInfItems = setKarteInfItems;
        SetOrderInfItems = setOrderInfItems;
        Status = status;
    }

    public List<SetByomeiItem> SetByomeiItems { get; private set; }

    public List<SetKarteInfModel> SetKarteInfItems { get; private set; }

    public List<SetOrderInfItem> SetOrderInfItems { get; private set; }

    public GetSuperSetDetailToDoTodayOrderStatus Status { get; private set; }
}
