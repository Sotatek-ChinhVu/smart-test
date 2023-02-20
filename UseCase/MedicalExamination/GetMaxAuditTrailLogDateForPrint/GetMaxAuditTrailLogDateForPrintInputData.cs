using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint
{
    public class GetMaxAuditTrailLogDateForPrintInputData : IInputData<GetMaxAuditTrailLogDateForPrintOutputData>
    {
        public GetMaxAuditTrailLogDateForPrintInputData(List<OdrInfItemInputData> odrInfs)
        {
            OdrInfs = odrInfs;
        }

        public List<OdrInfItemInputData> OdrInfs { get; private set; }
    }
}
