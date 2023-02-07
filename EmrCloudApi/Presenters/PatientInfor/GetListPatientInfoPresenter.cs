using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using UseCase.PatientInfor.GetListPatient;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor;

public class GetListPatientInfoPresenter : IGetListPatientInfoOutputPort
{
    public Response<GetListPatientInfoResponse> Result { get; private set; } = new Response<GetListPatientInfoResponse>();
    public void Complete(GetListPatientInfoOutputData outputData)
    {
        Result.Data = new GetListPatientInfoResponse(outputData.PatientInfoLists);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }
    private string GetMessage(GetListPatientInfoStatus status) => status switch
    {
        GetListPatientInfoStatus.Success => ResponseMessage.Success,
        GetListPatientInfoStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetListPatientInfoStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        GetListPatientInfoStatus.InvalidPageIndex => ResponseMessage.InvalidPageIndex,
        GetListPatientInfoStatus.InvalidPageSize => ResponseMessage.InvalidPageSize,
        _ => string.Empty
    };
}
