using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaCmtMst
{
    public class GetListKensaCmtMstInputData : IInputData<GetListKensaCmtMstOutputData>
    {
        public GetListKensaCmtMstInputData(int hpId, int iraiCd, string keyword)
        {
            HpId = hpId;
            IraiCd = iraiCd;
            Keyword = keyword;
        }

        public int HpId { get; private set; }

        public int IraiCd { get; private set; }

        public string Keyword{ get; private set; }
    }
}
