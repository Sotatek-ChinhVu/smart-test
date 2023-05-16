using Helper.Common;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetInsuranceReceInfListResponse
{
    public GetInsuranceReceInfListResponse(string hokenName, int sinYm, InsuranceReceInfItem insuranceReceInf)
    {
        HokenName = hokenName;
        InsuranceReceInf = insuranceReceInf;
        SinYmDisplay = CIUtil.SMonthToShowSWMonth(sinYm, 1);
    }

    public InsuranceReceInfItem InsuranceReceInf { get; private set; }

    public string HokenName { get; private set; }

    public string SinYmDisplay { get; private set; }
}
