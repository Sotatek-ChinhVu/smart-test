using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.GroupInf;
using UseCase.GroupInf.GetList;

namespace EmrCloudApi.Tenant.Presenters.GroupInf
{
    public class GetListGroupInfPresenter : IGetListGroupInfOutputPort
    {
        public Response<GetListGroupInfResponse> Result { get; private set; } = default!;

        public void Complete(GetListGroupInfOutputData outputData)
        {
            Result = new Response<GetListGroupInfResponse>
            {
                Data = new GetListGroupInfResponse(outputData.ListGroup),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListGroupInfSatus.Successed:
                    Result.Message = ResponseMessage.GetGroupInfSuccessed;
                    break;
                case GetListGroupInfSatus.InValidHpId:
                    Result.Message = ResponseMessage.GetGroupInfInvalidHpId;
                    break;
                case GetListGroupInfSatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetGroupInfInvalidPtId;
                    break;
                default:
                    break;
            }
        }
    }
}
