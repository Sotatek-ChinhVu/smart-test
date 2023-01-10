using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.User;
using UseCase.User.CheckedLockMedicalExamination;

namespace EmrCloudApi.Presenters.User;

public class CheckedLockMedicalExaminationPresenter : ICheckedLockMedicalExaminationOutputPort
{
    public Response<CheckedLockMedicalExaminationResponse> Result { get; private set; } = new Response<CheckedLockMedicalExaminationResponse>();

    public void Complete(CheckedLockMedicalExaminationOutputData output)
    {
        Result.Data = new CheckedLockMedicalExaminationResponse(output.IsLocked);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(CheckedLockMedicalExaminationStatus status) => status switch
    {
        CheckedLockMedicalExaminationStatus.Successed => ResponseMessage.Success,
        CheckedLockMedicalExaminationStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        CheckedLockMedicalExaminationStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        CheckedLockMedicalExaminationStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        CheckedLockMedicalExaminationStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        CheckedLockMedicalExaminationStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
        CheckedLockMedicalExaminationStatus.InvalidToken => ResponseMessage.InvalidToken,
        CheckedLockMedicalExaminationStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
