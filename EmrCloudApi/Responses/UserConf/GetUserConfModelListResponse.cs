using Domain.Models.UserConf;

namespace EmrCloudApi.Responses.UserConf
{
    public class GetUserConfModelListResponse
    {
        public GetUserConfModelListResponse(List<UserConfModel> userConfs)
        {
            UserConfs = userConfs;
        }

        public List<UserConfModel> UserConfs { get; private set; }
    }
}
