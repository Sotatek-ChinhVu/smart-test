using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.ValidationTodayOrd;

namespace UseCase.OrdInfs.CheckedSpecialItem
{
    public class CheckedSpecialItemInputData : IInputData<CheckedSpecialItemOutputData>
    {
        public CheckedSpecialItemInputData(int hpId, long ptId, int sinDate, int iBirthDay, int checkAge, long raiinNo, List<OdrInfDetailItemInputData> odrInfs, CheckedSpecialItemStatus status)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            IBirthDay = iBirthDay;
            CheckAge = checkAge;
            RaiinNo = raiinNo;
            OdrInfDetails = odrInfs;
            Status = status;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int IBirthDay { get; private set; }
        public int CheckAge { get; private set; }
        public long RaiinNo { get; private set; }
        public List<OdrInfDetailItemInputData> OdrInfDetails { get; private set; }
        public CheckedSpecialItemStatus Status { get; private set; }
    }
}
