using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetMaxRpNo
{
    public class GetMaxRpNoOutputData : IOutputData
    {
        public GetMaxRpNoOutputData(long maxRpNo, GetMaxRpNoStatus status)
        {
            MaxRpNo = maxRpNo;
            Status = status;
        }

        public long MaxRpNo { get; private set; }
        public GetMaxRpNoStatus Status { get; private set; }
    }
}
