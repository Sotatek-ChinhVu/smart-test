using UseCase.Core.Sync.Core;

namespace UseCase.UsageTreeSet.GetTree
{
    public class GetUsageTreeSetInputData : IInputData<GetUsageTreeSetOutputData>
    {
        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public int KouiKbn { get; private set; }

        public GetUsageTreeSetInputData(int hpId, int sinDate, int kouiKbn)
        {
            HpId = hpId;
            SinDate = sinDate;
            KouiKbn = kouiKbn;
        }
    }
}