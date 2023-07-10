using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetDefaultSelectPattern
{
    public class GetDefaultSelectPatternOutputData : IOutputData
    {
        public List<GetDefaultSelectPatternItem> HokenPids { get; private set; }

        public GetDefaultSelectPatternStatus Status { get; private set; }

        public GetDefaultSelectPatternOutputData(List<GetDefaultSelectPatternItem> hokenPids, GetDefaultSelectPatternStatus status)
        {
            HokenPids = hokenPids;
            Status = status;
        }
    }

    public class GetDefaultSelectPatternItem
    {
        public GetDefaultSelectPatternItem(int hokenPId, int changeHokenPId)
        {
            HokenPId = hokenPId;
            ChangeHokenPId = changeHokenPId;
        }

        public int HokenPId { get; private set; }

        public int ChangeHokenPId { get; private set; }
    }
}