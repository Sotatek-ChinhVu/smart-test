using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListResultKensaMst
{
    public class GetListKensaMstInputData : IInputData<GetListKensaMstOuputData>
    {
        public int HpId { get; private set; }
        public string Keyword { get; private set; }
        public GetListKensaMstInputData(int hpId, string keyword)
        {
            HpId = hpId;
            Keyword = keyword;
        }
    }
}
