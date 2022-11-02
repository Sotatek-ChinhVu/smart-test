using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetDefaultSelectPattern
{
    public class GetDefaultSelectPatternInputData : IInputData<GetDefaultSelectPatternOutputData>
    {
        public GetDefaultSelectPatternInputData(int hpId, long ptId, int sinDate, int historyPid, int selectedHokenPid)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            HistoryPid = historyPid;
            SelectedHokenPid = selectedHokenPid;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int HistoryPid { get; private set; }

        public int SelectedHokenPid { get; private set; }
    }
}