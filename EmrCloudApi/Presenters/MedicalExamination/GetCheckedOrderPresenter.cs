using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetCheckedOrder;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetCheckedOrderPresenter : IGetCheckedOrderOutputPort
    {
        public Response<GetCheckedOrderResponse> Result { get; private set; } = default!;

        public void Complete(GetCheckedOrderOutputData outputData)
        {
            Result = new Response<GetCheckedOrderResponse>()
            {
                Data = new GetCheckedOrderResponse(outputData.CheckedOrderModels),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetCheckedOrderStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetCheckedOrderStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetCheckedOrderStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetCheckedOrderStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetCheckedOrderStatus.InvalidIBirthDay:
                    Result.Message = ResponseMessage.InvalidIBirthDay;
                    break;
                case GetCheckedOrderStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case GetCheckedOrderStatus.InvalidHokenId:
                    Result.Message = ResponseMessage.InvalidHokenId;
                    break;
                case GetCheckedOrderStatus.InvalidSyosaisinKbn:
                    Result.Message = ResponseMessage.RaiinInfTodayOdrInvalidSyosaiKbn;
                    break;
                case GetCheckedOrderStatus.InvalidOyaRaiinNo:
                    Result.Message = ResponseMessage.InvalidOyaRaiinNo;
                    break;
                case GetCheckedOrderStatus.InvalidTantoId:
                    Result.Message = ResponseMessage.InvalidTantoId;
                    break;
                case GetCheckedOrderStatus.InvalidPrimaryDoctor:
                    Result.Message = ResponseMessage.InvalidPrimaryDoctor;
                    break;
                case GetCheckedOrderStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetCheckedOrderStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
