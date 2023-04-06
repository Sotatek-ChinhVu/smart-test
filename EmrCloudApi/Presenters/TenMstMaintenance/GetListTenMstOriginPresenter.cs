using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.TenMstMaintenance.GetListTenMstOrigin;
using EmrCloudApi.Responses.GetListTenMstOrigin;

namespace EmrCloudApi.Presenters.TenMstMaintenance
{
    public class GetListTenMstOriginPresenter : IGetListTenMstOriginOutputPort
    {
        public Response<GetListTenMstOriginResponse> Result { get; private set; } = default!;

        public void Complete(GetListTenMstOriginOutputData outputData)
        {
            Result = new Response<GetListTenMstOriginResponse>()
            {
                Data = new GetListTenMstOriginResponse(outputData.TenMsts, outputData.StartDateDisplay),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListTenMstOriginStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetListTenMstOriginStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetListTenMstOriginStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
