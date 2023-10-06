using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.SaveKensaIrai;

namespace EmrCloudApi.Presenters.MedicalExamination;

public class SaveKensaIraiPresenter : ISaveKensaIraiOutputPort
{
    public Response<SaveKensaIraiResponse> Result { get; private set; } = new();

    public void Complete(SaveKensaIraiOutputData output)
    {
        Result.Data = new SaveKensaIraiResponse(output.Message, output.KensaIraiReportItemList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveKensaIraiStatus status) => status switch
    {
        SaveKensaIraiStatus.Successed => ResponseMessage.Success,
        SaveKensaIraiStatus.Failed => ResponseMessage.Failed,
        SaveKensaIraiStatus.IsDeleteFile => ResponseMessage.IsDeleteFile,
        _ => string.Empty
    };
}

