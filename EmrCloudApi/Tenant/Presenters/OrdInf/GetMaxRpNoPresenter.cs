using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using UseCase.OrdInfs.GetMaxRpNo;

namespace EmrCloudApi.Tenant.Presenters.OrdInfs
{
    public class GetMaxRpNoPresenter : IGetMaxRpNoOutputPort
    {
        public Response<GetMaxRpNoResponse> Result { get; private set; } = default!;

        public void Complete(GetMaxRpNoOutputData outputData)
        {
            Result = new Response<GetMaxRpNoResponse>()
            {
                Data = new GetMaxRpNoResponse(outputData.MaxRpNo),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetMaxRpNoStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetMaxRpNoStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetMaxRpNoStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetMaxRpNoStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetMaxRpNoStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetMaxRpNoStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }

        }
    }
}
