using EmrCloudApi.Constants;
using EmrCloudApi.Responses.PatientInfor;
using EmrCloudApi.Responses;
using UseCase.PatientInfor.CheckValidSamePatient;

namespace EmrCloudApi.Presenters.PatientInfor
{
    public class CheckValidSamePatientPresenter : ICheckValidSamePatientOutputPort
    {
        public Response<CheckValidSamePatientResponse> Result { get; private set; } = new Response<CheckValidSamePatientResponse>();

        public void Complete(CheckValidSamePatientOutputData outputData)
        {
            Result.Data = new CheckValidSamePatientResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = string.IsNullOrEmpty(outputData.Message) ? GetMessage(outputData.Status) : outputData.Message;
        }

        private string GetMessage(CheckValidSamePatientStatus status) => status switch
        {
            CheckValidSamePatientStatus.InvalidKanjiName => ResponseMessage.InvalidKanjiName,
            CheckValidSamePatientStatus.InvalidBirthday => ResponseMessage.InvalidBirthDay,
            CheckValidSamePatientStatus.InvalidSex => ResponseMessage.InvalidSex,
            CheckValidSamePatientStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            CheckValidSamePatientStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            CheckValidSamePatientStatus.IsValid => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
