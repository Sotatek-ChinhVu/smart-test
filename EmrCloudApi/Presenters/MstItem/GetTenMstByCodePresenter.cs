using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTenMstByCode;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTenMstByCodePresenter : IGetTenMstByCodeOutputPort
    {
        public Response<GetTenMstByCodeResponse> Result { get; private set; } = default!;

        public void Complete(GetTenMstByCodeOutputData outputData)
        {
            Result = new Response<GetTenMstByCodeResponse>()
            {
                Data = new GetTenMstByCodeResponse(outputData.TenMst),
                Status = (int)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetTenMstByCodeStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetTenMstByCodeStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetTenMstByCodeStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
            }
        }
    }
}
