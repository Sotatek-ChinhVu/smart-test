using EmrCloudApi.Responses.PatientInfor;
using EmrCloudApi.Responses;
using UseCase.PatientInfor.DeletePatient;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.PatientInfor
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
            DeletePatientInfoStatus.NotAllowDeletePatient => ResponseMessage.NotAllowDeletePatient,
            _ => string.Empty
        };
    }
}
