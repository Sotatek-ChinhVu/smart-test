using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor.PtKyuseiInf;
using UseCase.PatientInfor.SavePtKyusei;

namespace EmrCloudApi.Presenters.PatientInfor.PtKyusei;

public class SavePtKyuseiPresenter
{
    public Response<SavePtKyuseiResponse> Result { get; private set; } = new();

    public void Complete(SavePtKyuseiOutputData output)
    {
        Result.Data = new SavePtKyuseiResponse(output.Status == SavePtKyuseiStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SavePtKyuseiStatus status) => status switch
    {
        SavePtKyuseiStatus.Successed => ResponseMessage.Success,
        SavePtKyuseiStatus.Failed => ResponseMessage.Failed,
        SavePtKyuseiStatus.InvalidPtId => ResponseMessage.NotFoundPtInf,
        SavePtKyuseiStatus.InvalidKanaName => ResponseMessage.UpsertInvalidKanaName,
        SavePtKyuseiStatus.InvalidName => ResponseMessage.UpsertInvalidName,
        SavePtKyuseiStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
        SavePtKyuseiStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
