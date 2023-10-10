using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetParrentKensaMst
{
    public class GetParrentKensaMstInputData : IInputData<GetParrentKensaMstOutputData>
    {
        public GetParrentKensaMstInputData(int hpId, string keyWord, string itemCd)
        {
            KeyWord = keyWord;
            HpId = hpId;
            ItemCd = itemCd;
        }

        public int HpId { get; private set; }

        public string KeyWord { get; private set; }

        public string ItemCd {  get; private set; }
    }
}
