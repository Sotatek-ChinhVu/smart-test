using Domain.Models.UserConf;

namespace EmrCloudApi.Responses.UserConf
{
    public class GetUserConfigParamResponse
    {
        public GetUserConfigParamResponse(List<UserConfModel> userConfs)
        {
            UserConfs = userConfs;
        }

        public List<UserConfModel> UserConfs { get; private set; }
    }
}
