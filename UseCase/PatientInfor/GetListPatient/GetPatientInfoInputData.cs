using Domain.Models.Diseases;
using Domain.Models.PatientInfor;
using Domain.Models.SpecialNote.PatientInfo;
using UseCase.Core.Sync.Core;
using UseCase.Diseases.Upsert;

namespace UseCase.PatientInfor.GetListPatient;

public class GetPatientInfoInputData : IInputData<GetPatientInfoOutputData>
{
    public GetPatientInfoInputData(int hpId, long ptId, int pageIndex, int pageSize)
    {
        HpId = hpId;
        PtId = ptId;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}