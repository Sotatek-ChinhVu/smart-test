using UseCase.Core.Sync.Core;

namespace UseCase.UsageTreeSet.GetTree
{
    public class GetUsageTreeSetInputData : IInputData<GetUsageTreeSetOutputData>
    {
        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public int SetUsageKbn { get; private set; }

        public GetUsageTreeSetInputData(int hpId, int sinDate, int setUsageKbn)
        {
            HpId = hpId;
            SinDate = sinDate;
            SetUsageKbn = setUsageKbn;
        }
    }
}