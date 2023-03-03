using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Schema.GetListInsuranceScan
{
    public class GetListInsuranceScanOutputData : IOutputData
    {
        public GetListInsuranceScanOutputData(GetListInsuranceScanStatus status, IEnumerable<InsuranceScanModel> datas)
        {
            Status = status;
            InsuranceScans = datas;
        }

        public GetListInsuranceScanStatus Status { get; private set; }

        public IEnumerable<InsuranceScanModel> InsuranceScans { get; private set; }
    }
}
