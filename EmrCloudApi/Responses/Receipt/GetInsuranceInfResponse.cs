using UseCase.Receipt.GetListReceInf;

namespace EmrCloudApi.Responses.Receipt
{
    public class GetInsuranceInfResponse
    {
        public GetInsuranceInfResponse(GetInsuranceInfStatus status)
        {
            Status = status;
        }

        public GetInsuranceInfStatus Status { get; private set; }
    }
}
