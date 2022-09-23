using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using UseCase.OrdInfs.GetHeaderInf;

namespace EmrCloudApi.Tenant.Presenters.OrdInfs
{
    public class GetHeaderInfPresenter : IGetHeaderInfOutputPort
    {
        public Response<GetHeaderInfResponse> Result { get; private set; } = default!;

        public void Complete(GetHeaderInfOutputData outputData)
        {
            Result = new Response<GetHeaderInfResponse>()
            {
                Data = new GetHeaderInfResponse(outputData.OdrInfs),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetHeaderInfStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetHeaderInfStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetHeaderInfStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetHeaderInfStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetHeaderInfStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetHeaderInfStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetHeaderInfStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }

        }
    }
}
