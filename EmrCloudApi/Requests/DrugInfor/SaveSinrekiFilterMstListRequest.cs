using EmrCloudApi.Requests.DrugInfor.SaveSinrekiFilterMstListRequestItem;

namespace EmrCloudApi.Requests.DrugInfor;

public class SaveSinrekiFilterMstListRequest
{
    public List<SinrekiFilterMstRequestItem> SinrekiFilterMstList { get; set; } = new();
}
