using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetInsuranceReceInfList;

public class GetInsuranceReceInfListOutputData : IOutputData
{
    public GetInsuranceReceInfListOutputData(string hokenName, int sinYm, InsuranceReceInfModel model, string insuranceName, GetInsuranceReceInfListStatus status)
    {
        HokenName = hokenName;
        InsuranceReceInf = new InsuranceReceInfItem(model, insuranceName);
        Status = status;
        SinYm = sinYm;
    }
    public string HokenName { get; private set; }

    public int SinYm { get; private set; }

    public InsuranceReceInfItem InsuranceReceInf { get; private set; }

    public GetInsuranceReceInfListStatus Status { get; private set; }
}
