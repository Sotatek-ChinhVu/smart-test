using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.AutoCheckOrder
{
    public class AutoCheckOrderInputData : IInputData<AutoCheckOrderOutputData>
    {
        public AutoCheckOrderInputData(int hpId, int sinDate, long ptId, List<OdrInfItemInputData> odrInfs)
        {
            HpId = hpId;
            SinDate = sinDate;
            PtId = ptId;
            OdrInfs = odrInfs;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public long PtId { get; private set; }

        public List<OdrInfItemInputData> OdrInfs { get; private set; }
    }
}
