using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.ConvertItem;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class ConvertItemPresenter : IConvertItemOutputPort
    {
        public Response<ConvertItemResponse> Result { get; private set; } = default!;

        public void Complete(ConvertItemOutputData outputData)
        {
            Result = new Response<ConvertItemResponse>()
            {
                Data = new ConvertItemResponse(outputData.Result),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ConvertItemStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ConvertItemStatus.InValidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ConvertItemStatus.InValidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case ConvertItemStatus.InValidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case ConvertItemStatus.InValidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case ConvertItemStatus.InputNotData:
                    Result.Message = ResponseMessage.InputNoData;
                    break;
                case ConvertItemStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case ConvertItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
