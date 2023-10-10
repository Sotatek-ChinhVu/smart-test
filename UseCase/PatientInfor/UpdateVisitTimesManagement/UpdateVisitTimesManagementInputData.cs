using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.UpdateVisitTimesManagement;

public class UpdateVisitTimesManagementInputData : IInputData<UpdateVisitTimesManagementOutputData>
{
    public UpdateVisitTimesManagementInputData(int hpId, int userId, int sinYm, long ptId, int kohiId, List<VisitTimesManagementModel> visitTimesManagementList)
    {
        HpId = hpId;
        UserId = userId;
        SinYm = sinYm;
        PtId = ptId;
        KohiId = kohiId;
        VisitTimesManagementList = visitTimesManagementList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }

    public int KohiId { get; private set; }

    public List<VisitTimesManagementModel> VisitTimesManagementList { get; private set; }
}
