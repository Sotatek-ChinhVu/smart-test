using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetHistoryIndex;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetHistoryIndexPresenter : IGetHistoryIndexOutputPort
    {
        public Response<GetHistoryIndexResponse> Result { get; private set; } = default!;

        public void Complete(GetHistoryIndexOutputData outputData)
        {
            Result = new Response<GetHistoryIndexResponse>()
            {
                Data = new GetHistoryIndexResponse(outputData.Index),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetHistoryIndexStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetHistoryIndexStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetHistoryIndexStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetHistoryIndexStatus.InvalidIsDeleted:
                    Result.Message = ResponseMessage.InvalidIsDeleted;
                    break;
                case GetHistoryIndexStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case GetHistoryIndexStatus.InvalidFilterId:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidFilterId;
                    break;
            }
        }
    }
}
