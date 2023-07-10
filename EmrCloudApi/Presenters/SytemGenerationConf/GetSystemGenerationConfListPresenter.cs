using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemGenerationConf;
using UseCase.SystemGenerationConf.GetList;

namespace EmrCloudApi.Presenters.SytemGenerationConf
{
    public class GetSystemGenerationConfListPresenter : IGetSystemGenerationConfListOutputPort
    {
        public Response<GetSystemGenerationConfListResponse> Result { get; private set; } = default!;

        public void Complete(GetSystemGenerationConfListOutputData outputData)
        {
            Result = new Response<GetSystemGenerationConfListResponse>()
            {
                Data = new GetSystemGenerationConfListResponse(outputData.SystemGenerationConfModels),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSystemGenerationConfListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
