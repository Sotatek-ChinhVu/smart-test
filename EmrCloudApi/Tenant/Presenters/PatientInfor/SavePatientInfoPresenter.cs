using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using UseCase.PatientInfor.Save;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor
{
    public class SavePatientInfoPresenter : ISavePatientInfoOutputPort
    {
        public Response<SavePatientInfoResponse> Result { get; private set; } = new Response<SavePatientInfoResponse>();

        public void Complete(SavePatientInfoOutputData outputData)
        {
            Result.Data = new SavePatientInfoResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
            if (outputData.Status == SavePatientInfoStatus.Failed)
                Result.Message += $".{outputData.Message}";
        }

        private string GetMessage(SavePatientInfoStatus status) => status switch
        {
            SavePatientInfoStatus.Successful => ResponseMessage.Success,
            SavePatientInfoStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}