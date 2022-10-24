using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor.InsuranceMasterLinkage;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using static Helper.Constants.DefHokenNoConst;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor.InsuranceMasterLinkage
{
    public class SaveInsuranceMasterLinkagePresenter : ISaveInsuranceMasterLinkageOutputPort
    {
        public Response<SaveInsuranceMasterLinkageResponse> Result { get; private set; } = new();

        public void Complete(SaveInsuranceMasterLinkageOutputData outputData)
        {
            Result.Data = new SaveInsuranceMasterLinkageResponse(outputData.Status == ValidationStatus.Success);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(ValidationStatus status) => status switch
        {
            ValidationStatus.Success => ResponseMessage.Success,
            ValidationStatus.Failed => ResponseMessage.Failed,
            ValidationStatus.InputDataNull => ResponseMessage.InputDataNull,
            ValidationStatus.InvalidDigit1 => ResponseMessage.InvalidDigit1,
            ValidationStatus.InvalidDigit2 => ResponseMessage.InvalidDigit2,
            ValidationStatus.InvalidDigit3 => ResponseMessage.InvalidDigit3,
            ValidationStatus.InvalidDigit4 => ResponseMessage.InvalidDigit4,
            ValidationStatus.InvalidDigit5 => ResponseMessage.InvalidDigit5,
            ValidationStatus.InvalidDigit6 => ResponseMessage.InvalidDigit6,
            ValidationStatus.InvalidDigit7 => ResponseMessage.InvalidDigit7,
            ValidationStatus.InvalidDigit8 => ResponseMessage.InvalidDigit8,
            ValidationStatus.InvalidHokenNo => ResponseMessage.InvalidHokenNo,
            ValidationStatus.InvalidHokenEdaNo => ResponseMessage.InvalidHokenEdaNo,
            _ => string.Empty,
        };
    }
}
