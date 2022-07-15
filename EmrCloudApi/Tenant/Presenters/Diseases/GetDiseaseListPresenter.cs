using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Diseases;
using UseCase.Diseases.GetDiseaseList;

namespace EmrCloudApi.Tenant.Presenters.Diseases
{
    public class GetDiseaseListPresenter : IGetDiseaseListOutputPort
    {
        public Response<GetDiseaseListResponse> Result { get; private set; } = new Response<GetDiseaseListResponse>();

        public void Complete(GetDiseaseListOutputData outputData)
        {
            Result.Data = new GetDiseaseListResponse
            {
                DiseaseList = outputData.DiseaseList
            };
        }
    }
}
