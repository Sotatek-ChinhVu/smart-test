using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetInsuranceReceInfListResponse
{
    public GetInsuranceReceInfListResponse(InsuranceReceInfItem insuranceReceInf)
    {
        InsuranceReceInf = insuranceReceInf;
    }

    public InsuranceReceInfItem InsuranceReceInf { get; private set; }
}
