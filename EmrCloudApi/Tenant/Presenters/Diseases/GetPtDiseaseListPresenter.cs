using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Diseases;
using UseCase.Diseases.GetDiseaseList;

namespace EmrCloudApi.Tenant.Presenters.Diseases
{
    public class GetPtDiseaseListPresenter : IGetPtDiseaseListOutputPort
    {
        public Response<GetPtDiseaseListResponse> Result { get; private set; } = new Response<GetPtDiseaseListResponse>();

        public void Complete(GetPtDiseaseListOutputData outputData)
        {
            Result = new Response<GetPtDiseaseListResponse>()
            {
                Data = new GetPtDiseaseListResponse()
                {
                    DiseaseList = outputData.DiseaseList
                },
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetPtDiseaseListStatus.PtDiseaseListNotExisted:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetPtDiseaseListStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
