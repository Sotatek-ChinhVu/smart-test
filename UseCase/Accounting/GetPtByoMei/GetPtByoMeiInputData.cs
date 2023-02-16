using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetPtByoMei
{
    public class GetPtByoMeiInputData : IInputData<GetPtByoMeiOutputData>
    {
        public GetPtByoMeiInputData(int hpId, long ptId, int sinDate)
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
