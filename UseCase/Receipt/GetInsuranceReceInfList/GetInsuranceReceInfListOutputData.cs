using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetInsuranceReceInfList;

public class GetInsuranceReceInfListOutputData : IOutputData
{
    public GetInsuranceReceInfListOutputData(InsuranceReceInfModel model, string insuranceName, GetInsuranceReceInfListStatus status)
    {
        InsuranceReceInf = new InsuranceReceInfItem(model, insuranceName);
        Status = status;
    }

    public InsuranceReceInfItem InsuranceReceInf { get; private set; }

    public GetInsuranceReceInfListStatus Status { get; private set; }
}
