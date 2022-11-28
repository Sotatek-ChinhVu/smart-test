using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientGroupMst.SaveList;

namespace EmrCloudApi.Presenters.PatientInfor;

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
        SaveListPatientGroupMstStatus.DuplicateGroupId => ResponseMessage.DuplicateGroupId,
        SaveListPatientGroupMstStatus.DuplicateGroupName => ResponseMessage.DuplicateGroupName,
        SaveListPatientGroupMstStatus.DuplicateGroupDetailCode => ResponseMessage.DuplicateGroupDetailCode,
        SaveListPatientGroupMstStatus.DuplicateGroupDetailName => ResponseMessage.DuplicateGroupDetailName,
        SaveListPatientGroupMstStatus.InvalidGroupId => ResponseMessage.InvalidGroupId,
        SaveListPatientGroupMstStatus.InvalidGroupName => ResponseMessage.InvalidGroupName,
        SaveListPatientGroupMstStatus.InvalidDetailGroupCode => ResponseMessage.InvalidDetailGroupCode,
        SaveListPatientGroupMstStatus.InvalidGroupDetailName => ResponseMessage.InvalidGroupDetailName,
        SaveListPatientGroupMstStatus.DuplicateGroupDetailSeqNo => ResponseMessage.DuplicateGroupDetailSeqNo,
        _ => string.Empty
    };
}
