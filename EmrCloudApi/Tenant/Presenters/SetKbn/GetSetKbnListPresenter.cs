using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetKbn;
using UseCase.SetKbn.GetList;

namespace EmrCloudApi.Tenant.Presenters.SetKbn
{
    public class GetSetKbnListPresenter : IGetSetKbnListOutputPort
    {
        public Response<GetSetKbnListResponse> Result { get; private set; } = default!;

        public void Complete(GetSetKbnListOutputData outputData)
        {
            Result = new Response<GetSetKbnListResponse>()
            {
                Data = new GetSetKbnListResponse(outputData.SetList),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSetKbnListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetSetListInvalidHpId;
                    break;
                case GetSetKbnListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetSetKbnListSinDate;
                    break;
                case GetSetKbnListStatus.InvalidSetKbnFrom:
                    Result.Message = ResponseMessage.GetSetKbnListInvalidSetKbnFrom;
                    break;
                case GetSetKbnListStatus.InvalidSetKbnTo:
                    Result.Message = ResponseMessage.GetSetKbnListInvalidSetKbnTo;
                    break;
                case GetSetKbnListStatus.Successed:
                    Result.Message = ResponseMessage.GetSetKbntListSuccessed;
                    break;
                case GetSetKbnListStatus.NoData:
                    Result.Message = ResponseMessage.GetSetKbnListNoData;
                    break;
                case GetSetKbnListStatus.InvalidSetKbn:
                    Result.Message = ResponseMessage.GetSetKbnListInvalidSetKbn;
                    break;
            }
        }
    }
}
