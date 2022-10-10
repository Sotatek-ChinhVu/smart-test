using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetHeaderInf
{
    public class GetHeaderInfInputData : IInputData<GetHeaderInfOutputData>
    {
        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }

        public GetHeaderInfInputData(long ptId, int hpId, long raiinNo, int sinDate)
        {
            PtId = ptId;
            HpId = hpId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
        }
    }
}
