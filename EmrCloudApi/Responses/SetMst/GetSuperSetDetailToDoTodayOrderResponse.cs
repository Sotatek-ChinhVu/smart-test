using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

namespace EmrCloudApi.Responses.SetMst;

public class GetSuperSetDetailToDoTodayOrderResponse
{
    public GetSuperSetDetailToDoTodayOrderResponse(List<SetByomeiItem> setByomeiItems, List<SetKarteInfModel> setKarteInfItems, List<SetOrderInfItem> setOrderInfItems)
    {
        SetByomeiItems = setByomeiItems;
        SetKarteInfItems = setKarteInfItems;
        SetOrderInfItems = setOrderInfItems;
    }

    public List<SetByomeiItem> SetByomeiItems { get; private set; }

    public List<SetKarteInfModel> SetKarteInfItems { get; private set; }

    public List<SetOrderInfItem> SetOrderInfItems { get; private set; }
}
