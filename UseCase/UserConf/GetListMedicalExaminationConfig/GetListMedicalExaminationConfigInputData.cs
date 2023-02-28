using UseCase.Core.Sync.Core;

namespace UseCase.UserConf.GetListMedicalExaminationConfig;

public class GetListMedicalExaminationConfigInputData : IInputData<GetListMedicalExaminationConfigOutputData>
{
    public GetListMedicalExaminationConfigInputData(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
