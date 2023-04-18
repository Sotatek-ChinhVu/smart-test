using UseCase.Core.Sync.Core;

namespace UseCase.UserConf.UserSettingParam
{
    public class GetUserConfigParamInputData : IInputData<GetUserConfigParamOutputData>
    {
        public GetUserConfigParamInputData(int hpId, int userId, List<Tuple<int, int>> groupCode)
        {
            HpId = hpId;
            UserId = userId;
            GroupCode = groupCode;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public List<Tuple<int, int>> GroupCode { get; private set; }
    }
}
