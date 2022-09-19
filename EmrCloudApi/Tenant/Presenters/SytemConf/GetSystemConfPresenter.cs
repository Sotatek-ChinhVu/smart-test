using EmrCloudApi.Tenant.Responses;
using UseCase.SystemConf;
using EmrCloudApi.Tenant.Responses.SystemConf;
using EmrCloudApi.Tenant.Constants;

namespace EmrCloudApi.Tenant.Presenters.SytemConf
{
    public class GetSystemConfPresenter : IGetSystemConfOutputPort
    {
        public Response<GetSystemConfResponse> Result { get; private set; } = default!;

        public void Complete(GetSystemConfOutputData outputData)
        {
            Result = new Response<GetSystemConfResponse>()
            {
                Data = new GetSystemConfResponse(outputData.Model),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSystemConfStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSystemConfStatus.InvalidGrpCd:
                    Result.Message = ResponseMessage.InvalidGrpCd;
                    break;
                case GetSystemConfStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
