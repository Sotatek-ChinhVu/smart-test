using Domain.Models.TodayOdr;
using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.OrdInfs.CheckedSpecialItem
{
    public class CheckedSpecialItemInputData : IInputData<CheckedSpecialItemOutputData>
    {
        public CheckedSpecialItemInputData(int hpId, long ptId, int sinDate, int iBirthDay, int checkAge, long raiinNo, List<OdrInfItemInputData> odrInfs, List<CheckedOrderModel> checkedOrderModels, KarteItemInputData karteInf, CheckedSpecialItemStatus status, bool enabledInputCheck, bool enabledCommentCheck)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            IBirthDay = iBirthDay;
            CheckAge = checkAge;
            RaiinNo = raiinNo;
            OdrInfs = odrInfs;
            Status = status;
            EnabledInputCheck = enabledInputCheck;
            EnabledCommentCheck = enabledCommentCheck;
            CheckedOrderModels = checkedOrderModels;
            KarteInf = karteInf;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int IBirthDay { get; private set; }

        public int CheckAge { get; private set; }

        public long RaiinNo { get; private set; }

        public bool EnabledInputCheck { get; private set; }

        public bool EnabledCommentCheck { get; private set; }

        public List<OdrInfItemInputData> OdrInfs { get; private set; }

        public List<CheckedOrderModel> CheckedOrderModels { get; private set; }

        public KarteItemInputData KarteInf { get; private set; }

        public CheckedSpecialItemStatus Status { get; private set; }
    }
}
