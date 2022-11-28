using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.InitDoctorCombo;

namespace EmrCloudApi.Presenters.Reception;

public class InitDoctorComboPresenter
{
    public Response<InitDoctorComboResponse> Result { get; private set; } = new();

    public void Complete(InitDoctorComboOutputData output)
    {
        Result.Data = new InitDoctorComboResponse(output.TantoId);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(InitDoctorComboStatus status) => status switch
    {
        InitDoctorComboStatus.Successed => ResponseMessage.Success,
        InitDoctorComboStatus.Failed => ResponseMessage.Failed,
        InitDoctorComboStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        InitDoctorComboStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        InitDoctorComboStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        InitDoctorComboStatus.InvalidUserId => ResponseMessage.ReceptionInvalidUserId,
        _ => string.Empty
    };
}
