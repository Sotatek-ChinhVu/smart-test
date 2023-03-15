using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;
using UseCase.Receipt.GetInsuranceInf;

namespace UseCase.Receipt.GetListReceInf
{
    public class GetInsuranceInfOutputData : IOutputData
    {
        public GetInsuranceInfOutputData(List<InsuranceInfDto> insuranceInfDtos, GetInsuranceInfStatus status)
        {
            InsuranceInfDtos = insuranceInfDtos;
            Status = status;
        }

        public List<InsuranceInfDto> InsuranceInfDtos { get; private set; }
        public GetInsuranceInfStatus Status { get; private set; }
    }
}
