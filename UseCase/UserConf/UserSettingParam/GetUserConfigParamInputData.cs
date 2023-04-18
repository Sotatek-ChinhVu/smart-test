using UseCase.Core.Sync.Core;

namespace UseCase.UserConf.UserSettingParam
{
    public class GetUserConfigParamInputData : IInputData<GetUserConfigParamOutputData>
    {
        public GetUserConfigParamInputData(int hpId, int userId, int grpCd, int grpItemCd)
        {
            HpId = hpId;
            UserId = userId;
            GrpCd = grpCd;
            GrpItemCd = grpItemCd;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int GrpCd { get; private set; }
        public int GrpItemCd { get; private set; }
    }
}
