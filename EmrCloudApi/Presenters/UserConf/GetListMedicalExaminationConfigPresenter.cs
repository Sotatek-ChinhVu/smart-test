using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using UseCase.UserConf.GetListMedicalExaminationConfig;

namespace EmrCloudApi.Presenters.UserConf;

public class GetListMedicalExaminationConfigPresenter : IGetListMedicalExaminationConfigOutputPort
{
    public Response<GetListMedicalExaminationConfigResponse> Result { get; private set; } = new();

    public void Complete(GetListMedicalExaminationConfigOutputData output)
    {
        Result.Data = new GetListMedicalExaminationConfigResponse(output);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListMedicalExaminationConfigStatus status) => status switch
    {
        GetListMedicalExaminationConfigStatus.Successed => ResponseMessage.Success,
        GetListMedicalExaminationConfigStatus.Failed => ResponseMessage.Failed,
        GetListMedicalExaminationConfigStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetListMedicalExaminationConfigStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        _ => string.Empty
    };
}
