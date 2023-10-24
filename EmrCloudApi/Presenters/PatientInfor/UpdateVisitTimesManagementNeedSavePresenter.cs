using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientInfor.UpdateVisitTimesManagementNeedSave;

namespace EmrCloudApi.Presenters.PatientInfor;

public class UpdateVisitTimesManagementNeedSavePresenter : IUpdateVisitTimesManagementNeedSaveOutputPort
{
    public Response<UpdateVisitTimesManagementNeedSaveResponse> Result { get; private set; } = new();

    public void Complete(UpdateVisitTimesManagementNeedSaveOutputData outputData)
    {
        Result.Data = new UpdateVisitTimesManagementNeedSaveResponse(outputData.Status == UpdateVisitTimesManagementNeedSaveStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(UpdateVisitTimesManagementNeedSaveStatus status) => status switch
    {
        UpdateVisitTimesManagementNeedSaveStatus.Successed => ResponseMessage.Success,
        UpdateVisitTimesManagementNeedSaveStatus.Failed => ResponseMessage.Failed,
        UpdateVisitTimesManagementNeedSaveStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}
