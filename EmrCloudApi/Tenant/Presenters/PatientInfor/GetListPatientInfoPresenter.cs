using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ApprovalInf;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using UseCase.ApprovalInfo.GetApprovalInfList;
using UseCase.PatientInfor.GetListPatient;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor;

public class GetListPatientInfoPresenter : IGetPatientInfoOutputPort
{
    public Response<GetListPatientInfoResponse> Result { get; private set; } = new Response<GetListPatientInfoResponse>();
    public void Complete(GetPatientInfoOutputData outputData)
    {
        Result.Data = new GetListPatientInfoResponse(outputData.PatientInfoList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }
    private string GetMessage(GetPatientInfoStatus status) => status switch
    {
        GetPatientInfoStatus.Success => ResponseMessage.Success,
        GetPatientInfoStatus.Failed => ResponseMessage.Failed,
        GetPatientInfoStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}
