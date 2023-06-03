using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetSinMei
{
    public class GetMeiHoGaiInputData : IInputData<GetMeiHoGaiOutputData>
    {
        public GetMeiHoGaiInputData(int hpId, long ptId, int sinDate, List<long> raiinNos)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNos = raiinNos;
        }
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public List<long> RaiinNos { get; private set; }
    }
}
