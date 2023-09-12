using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetParrentKensaMst
{
    public class GetParrentKensaMstInputData : IInputData<GetParrentKensaMstOutputData>
    {
        public GetParrentKensaMstInputData(int hpId, string keyWord)
        {
            KeyWord = keyWord;
            HpId = hpId;
        }

        public int HpId { get; private set; }

        public string KeyWord { get; private set; }
    }
}
