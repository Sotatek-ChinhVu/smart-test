using Domain.Models.UserConf;
using UseCase.Core.Sync.Core;

namespace UseCase.UserConf.UserSettingParam
{
    public class GetUserConfigParamOutputData : IOutputData
    {
        public GetUserConfigParamOutputData(List<UserConfModel> userConfs, GetUserConfigParamStatus status)
        {
            UserConfs = userConfs;
            Status = status;
        }

        public List<UserConfModel> UserConfs { get; private set; }
        public GetUserConfigParamStatus Status { get; private set; }
    }
}
