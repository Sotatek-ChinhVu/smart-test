using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.HokenMst;
using UseCase.HokenMst.GetDetail;

namespace EmrCloudApi.Tenant.Presenters.HokenMst
{
    public class GetDetailHokenMstPresenter : IGetDetailHokenMstOutputPort
    {
        public Response<GetDetailHokenMstResponse> Result { get; private set; } = new Response<GetDetailHokenMstResponse>();
        public void Complete(GetDetailHokenMstOutputData outputData)
        {
            Result.Data = new GetDetailHokenMstResponse(outputData.Data);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetDetailHokenMstStatus status) => status switch
        {
            GetDetailHokenMstStatus.Successed => ResponseMessage.Success,
            GetDetailHokenMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetDetailHokenMstStatus.InvalidHokenEdaNo => ResponseMessage.DetailHokenMstInvalidHokenEdaNo,
            GetDetailHokenMstStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            GetDetailHokenMstStatus.InvalidHokenNo => ResponseMessage.DetailHokenMstInvalidHokenNo,
            GetDetailHokenMstStatus.InvalidPrefNo => ResponseMessage.DetailHokenMstInvalidPrefNo,
            GetDetailHokenMstStatus.Exception => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
