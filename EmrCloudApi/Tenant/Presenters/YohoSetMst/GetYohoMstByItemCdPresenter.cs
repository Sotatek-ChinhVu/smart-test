using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.YohoSetMst;
using UseCase.YohoSetMst.GetByItemCd;

namespace EmrCloudApi.Tenant.Presenters.YohoSetMst
{
    public class GetYohoMstByItemCdPresenter : IGetYohoMstByItemCdOutputPort
    {
        public Response<GetYohoSetMstByItemCdResponse> Result { get; private set; } = default!;

        public void Complete(GetYohoMstByItemCdOutputData outputData)
        {
            Result = new Response<GetYohoSetMstByItemCdResponse>
            {
                Data = new GetYohoSetMstByItemCdResponse()
                {
                    Data = outputData.YohoSetMsts.Select(x=> new YohoSetMstDto(x)).ToList()
                },
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetYohoMstByItemCdStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;

                case GetYohoMstByItemCdStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;

                case GetYohoMstByItemCdStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;

                case GetYohoMstByItemCdStatus.InvalidStartDate:
                    Result.Message = ResponseMessage.InvalidStartDate;
                    break;

                case GetYohoMstByItemCdStatus.DataNotFound:
                    Result.Message = ResponseMessage.NotFound;
                    break;
            }
        }
    }
}
