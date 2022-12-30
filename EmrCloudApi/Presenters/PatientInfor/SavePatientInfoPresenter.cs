using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientInfor.Save;

namespace EmrCloudApi.Presenters.PatientInfor
{
    public class SavePatientInfoPresenter : ISavePatientInfoOutputPort
    {
        public Response<SavePatientInfoResponse> Result { get; private set; } = new Response<SavePatientInfoResponse>();

        public void Complete(SavePatientInfoOutputData outputData)
        {
            Result.Data = new SavePatientInfoResponse(outputData.ValidateDetails, outputData.Status, outputData.PtID);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SavePatientInfoStatus status) => status switch
        {
            SavePatientInfoStatus.Successful => ResponseMessage.Success,
            SavePatientInfoStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}