using UseCase.Core.Sync.Core;

namespace UseCase.WeightedSetConfirmation.Save
{
    public class SaveWeightedSetConfirmationInputData : IInputData<SaveWeightedSetConfirmationOutputData>
    {
        public SaveWeightedSetConfirmationInputData(int hpId, long ptId, long raiinNo, double weithgt, int sinDate, int userId)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            Weithgt = weithgt;
            SinDate = sinDate;
            UserId = userId;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long RaiinNo { get; private set; }

        public double Weithgt { get; private set; }

        public int SinDate { get; private set; }

        public int UserId { get; private set; }
    }
}
