using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListReceInf
{
    public class GetInsuranceInfOutputData : IOutputData
    {
        public GetInsuranceInfOutputData(GetInsuranceInfStatus status)
        {
            Status = status;
        }

        public GetInsuranceInfStatus Status { get; private set; }
    }
}
