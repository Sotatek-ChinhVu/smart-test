using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListResultKensaMst
{
    public class GetListResultKensaMstInputData : IInputData<GetListKensaMstOuputData>
    {
        public int HpId { get; set; }
        public string Keyword { get; set; }
        public GetListResultKensaMstInputData(int hpId, string keyword)
        {
            HpId = hpId;
            Keyword = keyword;
        }
    }
}
