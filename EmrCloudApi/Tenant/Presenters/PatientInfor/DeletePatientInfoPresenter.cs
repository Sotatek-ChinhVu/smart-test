using EmrCloudApi.Tenant.Responses.PatientInfor;
using EmrCloudApi.Tenant.Responses;
using UseCase.PatientInfor.DeletePatient;
using EmrCloudApi.Tenant.Constants;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor
{
    public class DeletePatientInfoPresenter : IDeletePatientInfoOutputPort
    {
        public Response<DeletePatientInfoResponse> Result { get; private set; } = new Response<DeletePatientInfoResponse>();

        public void Complete(DeletePatientInfoOutputData outputData)
        {
            Result.Data = new DeletePatientInfoResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(DeletePatientInfoStatus status) => status switch
        {
            DeletePatientInfoStatus.Successful => ResponseMessage.Success,
            DeletePatientInfoStatus.Failed => ResponseMessage.Failed,
            DeletePatientInfoStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            DeletePatientInfoStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            _ => string.Empty
        };
    }
}
