using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Diseases;
using EmrCloudApi.Responses;
using UseCase.Diseases.GetTreeSetByomei;

namespace EmrCloudApi.Presenters.Diseases
{
    public class GetTreeSetByomeiPresenter : IGetTreeSetByomeiOutputPort
    {
        public Response<GetTreeSetByomeiResponse> Result { get; private set; } = new Response<GetTreeSetByomeiResponse>();

        public void Complete(GetTreeSetByomeiOutputData outputData)
        {
            Result = new Response<GetTreeSetByomeiResponse>()
            {
                Data = new GetTreeSetByomeiResponse()
                {
                    Datas = outputData.Datas
                },
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTreeSetByomeiStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetTreeSetByomeiStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetTreeSetByomeiStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetTreeSetByomeiStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
