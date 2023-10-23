using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaCmtMst.GetKensaInfDetailByIraiCd
{
    public class GetKensaInfDetailByIraiCdInputData : IInputData<GetKensaInfDetailByIraiCdOutputData>
    {
        public GetKensaInfDetailByIraiCdInputData(int hpId, int iraiCd)
        {
            HpId = hpId;
            IraiCd = iraiCd;
        }

        public int HpId { get; private set; }
        public int IraiCd { get; private set; }
    }
}
