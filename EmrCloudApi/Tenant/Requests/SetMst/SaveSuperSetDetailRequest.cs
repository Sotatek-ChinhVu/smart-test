using UseCase.SuperSetDetail.SaveSuperSetDetail;

namespace EmrCloudApi.Tenant.Requests.SetMst;

public class SaveSuperSetDetailRequest
{
    public int SetCd { get; set; }

    public int UserId { get; set; }

    public SaveSuperSetDetailInputItem SaveSuperSetDetailInput { get; set; } = new();
}
