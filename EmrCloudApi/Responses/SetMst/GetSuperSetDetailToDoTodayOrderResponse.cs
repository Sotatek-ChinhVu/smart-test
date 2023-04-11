using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

namespace EmrCloudApi.Responses.SetMst;

public class GetSuperSetDetailToDoTodayOrderResponse
{
    public GetSuperSetDetailToDoTodayOrderResponse(List<SetByomeiItem> setByomeiItems, List<SetKarteInfModel> setKarteInfItems, List<SetOrderInfItem> setOrderInfItems, List<SetFileInfModel> setFileInfModels)
    {
        SetByomeiItems = setByomeiItems;
        SetKarteInfItems = setKarteInfItems;
        SetOrderInfItems = setOrderInfItems;
        SetFileInfModels = setFileInfModels;
    }

    public List<SetByomeiItem> SetByomeiItems { get; private set; }

    public List<SetKarteInfModel> SetKarteInfItems { get; private set; }

    public List<SetOrderInfItem> SetOrderInfItems { get; private set; }

    public List<SetFileInfModel> SetFileInfModels { get; private set; }
}
