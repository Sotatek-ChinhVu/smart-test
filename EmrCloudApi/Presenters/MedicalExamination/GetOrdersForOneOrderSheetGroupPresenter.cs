using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetOrdersForOneOrderSheetGroup;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetOrdersForOneOrderSheetGroupPresenter : IGetOrdersForOneOrderSheetGroupOutputPort
    {
        public Response<GetOrdersForOneOrderSheetGroupResponse> Result { get; private set; } = default!;

        public void Complete(GetOrdersForOneOrderSheetGroupOutputData outputData)
        {
            Result = new Response<GetOrdersForOneOrderSheetGroupResponse>()
            {
                Data = new GetOrdersForOneOrderSheetGroupResponse(outputData.Total, outputData.RaiinfList),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetOrdersForOneOrderSheetGroupStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetOrdersForOneOrderSheetGroupStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetOrdersForOneOrderSheetGroupStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetOrdersForOneOrderSheetGroupStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetOrdersForOneOrderSheetGroupStatus.InvalidOffset:
                    Result.Message = ResponseMessage.InvalidStartIndex;
                    break;
                case GetOrdersForOneOrderSheetGroupStatus.InvalidLimit:
                    Result.Message = ResponseMessage.InvalidPageSize;
                    break;
                case GetOrdersForOneOrderSheetGroupStatus.InvalidOdrKouiKbn:
                    Result.Message = ResponseMessage.InvalidDetailGroupCode;
                    break;
            }
        }
    }
}
