using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListKensaIjiSetting
{
    public sealed class GetListKensaIjiSettingInputData : IInputData<GetListKensaIjiSettingOutputData>
    {
        public GetListKensaIjiSettingInputData(int hpId, string keyWords, bool isValid, bool isExpired)
        {
            HpId = hpId;
            KeyWords = keyWords;
            IsValid = isValid;
            IsExpired = isExpired;
        }
        public int HpId {  get; private set; }
        public string KeyWords { get; private set; }
        public bool IsValid { get; private set; }
        public bool IsExpired { get; private set; }
    }
}
