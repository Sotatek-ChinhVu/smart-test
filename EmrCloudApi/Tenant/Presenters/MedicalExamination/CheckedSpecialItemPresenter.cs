using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetHistory;
using UseCase.OrdInfs.CheckedSpecialItem;

namespace EmrCloudApi.Tenant.Presenters.MedicalExamination
{
    public class CheckedSpecialItemPresenter : ICheckedSpecialItemOutputPort
    {
        public Response<CheckedSpecialItemResponse> Result { get; private set; } = default!;

        public void Complete(CheckedSpecialItemOutputData outputData)
        {
            Result = new Response<CheckedSpecialItemResponse>()
            {
                Data = new CheckedSpecialItemResponse(outputData.CheckSpecialItemModels),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case CheckedSpecialItemStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case CheckedSpecialItemStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case CheckedSpecialItemStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case CheckedSpecialItemStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case CheckedSpecialItemStatus.InvalidIBirthDay:
                    Result.Message = ResponseMessage.InvalidIBirthDay;
                    break;
                case CheckedSpecialItemStatus.InvalidCheckAge:
                    Result.Message = ResponseMessage.InvalidCheckAge;
                    break;
               case CheckedSpecialItemStatus.InvalidOdrInfDetail:
                    Result.Message = ResponseMessage.InvalidOdrInfDetail;
                    break;
                case CheckedSpecialItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case CheckedSpecialItemStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
