using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using UseCase.Insurance.GetComboList;

namespace EmrCloudApi.Responses.InsuranceList
{
    public class GetInsuranceComboListResponse
    {
        public GetInsuranceComboListResponse(List<GetInsuranceComboItemOuputData> data)
        {
            Data = data;
        }

        public List<GetInsuranceComboItemOuputData> Data { get; private set; } = new();
    }
}