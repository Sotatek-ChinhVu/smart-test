using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.CheckNextOrdHaveOdr
{
    public class CheckNextOrdHaveOdrInputData : IInputData<CheckNextOrdHaveOdrOutputData>
    {
        public CheckNextOrdHaveOdrInputData(long ptId, int hpId, int sinDate)
        {
            PtId = ptId;
            HpId = hpId;
            SinDate = sinDate;
        }

        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }
    }
}
