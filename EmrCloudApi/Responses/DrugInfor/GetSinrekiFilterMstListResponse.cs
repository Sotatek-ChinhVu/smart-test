using EmrCloudApi.Responses.DrugInfor.Dto;

namespace EmrCloudApi.Responses.DrugInfor;

public class GetSinrekiFilterMstListResponse
{
    public GetSinrekiFilterMstListResponse(List<SinrekiFilterMstDto> sinrekiFilterMstList)
    {
        SinrekiFilterMstList = sinrekiFilterMstList;
    }

    public List<SinrekiFilterMstDto> SinrekiFilterMstList { get; private set; }
}
