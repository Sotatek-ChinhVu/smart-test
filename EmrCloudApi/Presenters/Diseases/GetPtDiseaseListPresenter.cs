using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Diseases;
using UseCase.Diseases.GetDiseaseList;

namespace EmrCloudApi.Presenters.Diseases
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
