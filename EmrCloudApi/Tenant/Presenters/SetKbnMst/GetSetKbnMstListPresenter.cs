using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetKbnMst;
using UseCase.SetKbnMst.GetList;

namespace EmrCloudApi.Tenant.Presenters.SetKbnMst
{
    public class GetSetKbnMstListPresenter : IGetSetKbnMstListOutputPort
    {
        public Response<GetSetKbnMstListResponse> Result { get; private set; } = default!;

        public void Complete(GetSetKbnMstListOutputData outputData)
        {
            Result = new Response<GetSetKbnMstListResponse>()
            {
                Data = new GetSetKbnMstListResponse(outputData.SetList),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSetKbnMstListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetSetListInvalidHpId;
                    break;
                case GetSetKbnMstListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetSetKbnListSinDate;
                    break;
                case GetSetKbnMstListStatus.InvalidSetKbnFrom:
                    Result.Message = ResponseMessage.GetSetKbnListInvalidSetKbnFrom;
                    break;
                case GetSetKbnMstListStatus.InvalidSetKbnTo:
                    Result.Message = ResponseMessage.GetSetKbnListInvalidSetKbnTo;
                    break;
                case GetSetKbnMstListStatus.Successed:
                    Result.Message = ResponseMessage.GetSetKbntListSuccessed;
                    break;
                case GetSetKbnMstListStatus.NoData:
                    Result.Message = ResponseMessage.GetSetKbnListNoData;
                    break;
                case GetSetKbnMstListStatus.InvalidSetKbn:
                    Result.Message = ResponseMessage.GetSetKbnListInvalidSetKbn;
                    break;
            }
        }
    }
}
