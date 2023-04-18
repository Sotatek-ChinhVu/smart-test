using UseCase.Core.Sync.Core;

namespace UseCase.UserConf.UserSettingParam
{
    public class GetUserConfigParamOutputData : IOutputData
    {
        public GetUserConfigParamOutputData(string param, GetUserConfigParamStatus status)
        {
            Param = param;
            Status = status;
        }

        public string Param { get; private set; }
        public GetUserConfigParamStatus Status { get; private set; }
    }
}
