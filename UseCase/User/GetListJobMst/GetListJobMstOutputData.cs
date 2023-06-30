using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetListJobMst
{
    public class GetListJobMstOutputData : IOutputData
    {
        public GetListJobMstOutputData(GetListJobMstStatus status, List<JobMstModel> jobMsts)
        {
            Status = status;
            JobMsts = jobMsts;
        }

        public GetListJobMstStatus Status { get; private set; }

        public List<JobMstModel> JobMsts { get; private set; }
    }
}
