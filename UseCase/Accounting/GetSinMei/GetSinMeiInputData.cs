using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetSinMei
{
    public class GetSinMeiInputData : IInputData<GetSinMeiOutputData>
    {
        public GetSinMeiInputData(int hpId, long ptId, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }
    }
}
