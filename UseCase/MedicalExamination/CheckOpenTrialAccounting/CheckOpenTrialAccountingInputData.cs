using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckOpenTrialAccounting
{
    public class CheckOpenTrialAccountingInputData : IInputData<CheckOpenTrialAccountingOutputData>
    {
        public CheckOpenTrialAccountingInputData(int hpId, int ptId, long raiinNo, int sinDate, int syosaiKbn, List<Tuple<string, string>> allOdrInfItem, List<int> odrInfHokenPid, List<string> itemCds)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            SyosaiKbn = syosaiKbn;
            AllOdrInfItem = allOdrInfItem;
            OdrInfHokenPid = odrInfHokenPid;
            ItemCds = itemCds;
        }

        public int HpId { get; private set; }
        public int PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public int SyosaiKbn { get; private set; }
        public List<Tuple<string, string>> AllOdrInfItem { get; private set; }
        public List<int> OdrInfHokenPid { get; private set; }
        public List<string> ItemCds { get; private set; }
    }
}
