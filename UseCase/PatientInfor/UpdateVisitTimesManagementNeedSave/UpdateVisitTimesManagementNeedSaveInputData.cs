using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.UpdateVisitTimesManagementNeedSave;

public class UpdateVisitTimesManagementNeedSaveInputData : IInputData<UpdateVisitTimesManagementNeedSaveOutputData>
{
    public UpdateVisitTimesManagementNeedSaveInputData(int hpId, int userId, long ptId, List<VisitTimesManagementModel> visitTimesManagementList)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        VisitTimesManagementList = visitTimesManagementList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public List<VisitTimesManagementModel> VisitTimesManagementList { get; private set; }
}
