using UseCase.Core.Sync.Core;

namespace UseCase.Schema.GetListInsuranceScan
{
    public class GetListInsuranceScanInputData : IInputData<GetListInsuranceScanOutputData>
    {
        public GetListInsuranceScanInputData(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }
    }
}
