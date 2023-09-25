using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListResultKensaMst
{
    public class GetListKensaMstInputData : IInputData<GetListKensaMstOuputData>
    {
        public int HpId { get; set; }
        public string Keyword { get; set; }
        public GetListKensaMstInputData(int hpId, string keyword)
        {
            HpId = hpId;
            Keyword = keyword;
        }
    }
}
