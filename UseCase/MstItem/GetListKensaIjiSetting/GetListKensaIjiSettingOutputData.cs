using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListKensaIjiSetting
{
    public sealed class GetListKensaIjiSettingOutputData : IOutputData
    {
        public GetListKensaIjiSettingOutputData( List<KensaIjiSettingModel> kensaIjiSettingModels, GetListKensaIjiSettingStatus status)
        {
            KensaIjiSettingModels = kensaIjiSettingModels;
            Status = status;
        }

        public List<KensaIjiSettingModel> KensaIjiSettingModels { get; private set; }
        public GetListKensaIjiSettingStatus Status { get; private set; }
    }
}
