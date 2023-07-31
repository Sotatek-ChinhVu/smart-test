using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Check
{
    public class CheckUpsertNextOrderInputData : IInputData<CheckUpsertNextOrderOutputData>
    {
        public CheckUpsertNextOrderInputData(int hpId, long ptId, int rsvDate)
        {
            HpId = hpId;
            PtId = ptId;
            RsvDate = rsvDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int RsvDate { get; private set; }

    }
}
