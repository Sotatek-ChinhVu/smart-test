using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.PatientInfor;

public class GetTokkiMstListResponse
{
    public GetTokkiMstListResponse(List<TokkiMstModel> tokkiMstList)
    {
        TokkiMstList = tokkiMstList.Select(item => new TokkiMstDto(item)).ToList();
    }

    public List<TokkiMstDto> TokkiMstList { get; private set; }
}
