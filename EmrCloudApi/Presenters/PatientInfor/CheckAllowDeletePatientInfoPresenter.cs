using EmrCloudApi.Constants;
using EmrCloudApi.Responses.PatientInfor;
using EmrCloudApi.Responses;
using UseCase.PatientInfor.CheckAllowDeletePatientInfo;

namespace EmrCloudApi.Presenters.PatientInfor
{
    public class CheckAllowDeletePatientInfoPresenter : ICheckAllowDeletePatientInfoOutputPort
    {
        public Response<CheckAllowDeletePatientInfoResponse> Result { get; private set; } = new Response<CheckAllowDeletePatientInfoResponse>();

        public void Complete(CheckAllowDeletePatientInfoOutputData outputData)
        {
            Result.Data = new CheckAllowDeletePatientInfoResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = !string.IsNullOrEmpty(outputData.Message) ? GetMessage(outputData.Status) : outputData.Message;
        }

        private string GetMessage(CheckAllowDeletePatientInfoStatus status) => status switch
        {
            CheckAllowDeletePatientInfoStatus.AllowDelete => ResponseMessage.Success,
            CheckAllowDeletePatientInfoStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            CheckAllowDeletePatientInfoStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            CheckAllowDeletePatientInfoStatus.NotAllowDelete => ResponseMessage.NotAllowDeletePatient,
            _ => string.Empty
        };
    }
}
