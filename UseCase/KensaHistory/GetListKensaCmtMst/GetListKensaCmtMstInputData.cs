using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaCmtMst
{
    public class GetListKensaCmtMstInputData : IInputData<GetListKensaCmtMstOutputData>
    {
        public GetListKensaCmtMstInputData(int hpId, string keyword)
        {
            HpId = hpId;
            Keyword = keyword;
        }

        public int HpId { get; set; }
        public string Keyword{ get; set; }
    }
}
