using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.Get
{
    public class GetInsuranceMstOutputData : IOutputData
    {
        public InsuranceMstModel InsuranceMstData { get; private set; }

        public GetInsuranceMstStatus Status { get; private set; }

        public int PrefNo { get; private set; }

        public GetInsuranceMstOutputData(InsuranceMstModel data, GetInsuranceMstStatus status, int prefNo)
        {
            InsuranceMstData = data;
            Status = status;
            PrefNo = prefNo;
        }
    }
}
