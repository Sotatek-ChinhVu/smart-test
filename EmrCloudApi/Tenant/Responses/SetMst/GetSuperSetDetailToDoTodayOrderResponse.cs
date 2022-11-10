using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

namespace EmrCloudApi.Tenant.Responses.SetMst;

public class GetSuperSetDetailToDoTodayOrderResponse
{
    public GetSuperSetDetailToDoTodayOrderResponse(List<SetByomeiItem> setByomeiItems, List<SetKarteInfItem> setKarteInfItems, List<SetOrderInfItem> setOrderInfItems)
    {
        SetByomeiItems = setByomeiItems;
        SetKarteInfItems = setKarteInfItems;
        SetOrderInfItems = setOrderInfItems;
    }

    public List<SetByomeiItem> SetByomeiItems { get; private set; }

    public List<SetKarteInfItem> SetKarteInfItems { get; private set; }

    public List<SetOrderInfItem> SetOrderInfItems { get; private set; }
}
