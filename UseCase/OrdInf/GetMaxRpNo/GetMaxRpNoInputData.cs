using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetMaxRpNo
{
    public class GetMaxRpNoInputData : IInputData<GetMaxRpNoOutputData>
    {
        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }

        public GetMaxRpNoInputData(long ptId, int hpId, long raiinNo, int sinDate)
        {
            PtId = ptId;
            HpId = hpId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
        }
    }
}
