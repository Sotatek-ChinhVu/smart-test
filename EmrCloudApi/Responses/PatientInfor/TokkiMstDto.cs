using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.PatientInfor;

public class TokkiMstDto
{
    public TokkiMstDto(TokkiMstModel model)
    {
        TokkiCd = model.TokkiCd;
        TokkiName = model.TokkiName;
        DisplayTextMst = model.DisplayTextMst;
    }

    public string TokkiCd { get; private set; }

    public string TokkiName { get; private set; }

    public string DisplayTextMst { get; private set; }
}
