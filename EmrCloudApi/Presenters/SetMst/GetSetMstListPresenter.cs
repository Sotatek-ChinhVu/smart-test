using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using UseCase.SetMst.GetList;

namespace EmrCloudApi.Presenters.SetMst
{
    public class GetSetMstListPresenter : IGetSetMstListOutputPort
    {
        public Response<GetSetMstListResponse> Result { get; private set; } = default!;

        public void Complete(GetSetMstListOutputData outputData)
        {
            Result = new Response<GetSetMstListResponse>()
            {
                Data = new GetSetMstListResponse(outputData.SetList),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSetMstListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetSetListInvalidHpId;
                    break;
                case GetSetMstListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetSetListSinDate;
                    break;
                case GetSetMstListStatus.InvalidSetKbn:
                    Result.Message = ResponseMessage.GetSetListInvalidSetKbn;
                    break;
                case GetSetMstListStatus.NoData:
                    Result.Message = ResponseMessage.GetSetListNoData;
                    break;
                case GetSetMstListStatus.Successed:
                    Result.Message = ResponseMessage.GetSetListSuccessed;
                    break;
                case GetSetMstListStatus.InvalidSetKbnEdaNo:
                    Result.Message = ResponseMessage.GetSetListInvalidSetKbn;
                    break;
            }
        }
    }
}
