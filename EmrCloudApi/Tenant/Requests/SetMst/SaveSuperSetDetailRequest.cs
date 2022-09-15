using UseCase.SupperSetDetail.SaveSuperSetDetail;

namespace EmrCloudApi.Tenant.Requests.SetMst;

public class SaveSuperSetDetailRequest
{
    public int SetCd { get; set; }

    public int UserId { get; set; }

    public SaveSuperSetDetailInputItem SaveSupperSetDetailInput { get; set; } = new();
}
