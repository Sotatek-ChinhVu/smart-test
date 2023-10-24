using EmrCloudApi.Constants;
using EmrCloudApi.Responses.PatientInfor;
using EmrCloudApi.Responses;
using UseCase.PatientInfor.UpdateVisitTimesManagement;

namespace EmrCloudApi.Presenters.PatientInfor;

public class UpdateVisitTimesManagementPresenter : IUpdateVisitTimesManagementOutputPort
{
    public Response<UpdateVisitTimesManagementResponse> Result { get; private set; } = new();

    public void Complete(UpdateVisitTimesManagementOutputData outputData)
    {
        Result.Data = new UpdateVisitTimesManagementResponse(outputData.Status == UpdateVisitTimesManagementStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(UpdateVisitTimesManagementStatus status) => status switch
    {
        UpdateVisitTimesManagementStatus.Successed => ResponseMessage.Success,
        UpdateVisitTimesManagementStatus.Failed => ResponseMessage.Failed,
        UpdateVisitTimesManagementStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        UpdateVisitTimesManagementStatus.CanNotDeleted => ResponseMessage.CanNotDeleted,
        _ => string.Empty
    };
}