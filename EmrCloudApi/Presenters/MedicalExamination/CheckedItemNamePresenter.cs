using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.CheckedItemName;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class CheckedItemNamePresenter : ICheckedItemNameOutputPort
    {
        public Response<CheckedItemNameResponse> Result { get; private set; } = default!;

        public void Complete(CheckedItemNameOutputData outputData)
        {
            Result = new Response<CheckedItemNameResponse>()
            {
                Data = new CheckedItemNameResponse(outputData.CheckedItemNames),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case CheckedItemNameStatus.InputNotData:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case CheckedItemNameStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case CheckedItemNameStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
