using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using UseCase.PatientGroupMst.SaveList;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor;

public class SaveListPatientGroupMstPresenter : ISaveListPatientGroupMstOutputPort
{
    public Response<SaveListPatientGroupMstResponse> Result { get; private set; } = new();

    public void Complete(SaveListPatientGroupMstOutputData output)
    {
        Result.Data = new SaveListPatientGroupMstResponse(output.Status == SaveListPatientGroupMstStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveListPatientGroupMstStatus status) => status switch
    {
        SaveListPatientGroupMstStatus.Successed => ResponseMessage.Success,
        SaveListPatientGroupMstStatus.Failed => ResponseMessage.Failed,
        SaveListPatientGroupMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveListPatientGroupMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,

        _ => string.Empty
    };
}
