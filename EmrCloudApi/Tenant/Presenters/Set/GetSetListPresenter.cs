using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Set;
using UseCase.Set.GetList;

namespace EmrCloudApi.Tenant.Presenters.Set
{
    public class GetSetListPresenter : IGetSetListOutputPort
    {
        public Response<GetSetListResponse> Result { get; private set; } = default!;

        public void Complete(GetSetListOutputData outputData)
        {
            Result = new Response<GetSetListResponse>()
            {
                Data = new GetSetListResponse(outputData.SetList),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSetListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetSetListInvalidHpId;
                    break;
                case GetSetListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetSetListSinDate;
                    break;
                case GetSetListStatus.InvalidSetKbn:
                    Result.Message = ResponseMessage.GetSetListInvalidSetKbn;
                    break;
                case GetSetListStatus.NoData:
                    Result.Message = ResponseMessage.GetSetListNoData;
                    break;
                case GetSetListStatus.Successed:
                    Result.Message = ResponseMessage.GetSetListSuccessed;
                    break;
                case GetSetListStatus.InvalidSetKbnEdaNo:
                    Result.Message = ResponseMessage.GetSetListInvalidSetKbn;
                    break;
            }
        }
    }
}
