using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Get
{
    public class GetLockInfoInputData : IInputData<GetLockInfoOutputData>
    {
        public GetLockInfoInputData(int hpId, long ptId, List<string> listFunctionCdB, int sinDate, long raiinNo)
        {
            HpId = hpId;
            PtId = ptId;
            ListFunctionCdB = listFunctionCdB;
            SinDate = sinDate;
            RaiinNo = raiinNo;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public List<string> ListFunctionCdB { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
    }
}
