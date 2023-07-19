using Domain.Models.User;

namespace EmrCloudApi.Responses.User
{
    public class GetListJobMstResponse
    {
        public GetListJobMstResponse(List<JobMstModel> jobMsts)
        {
            JobMsts = jobMsts;
        }

        public List<JobMstModel> JobMsts { get; private set; }
    }
}
