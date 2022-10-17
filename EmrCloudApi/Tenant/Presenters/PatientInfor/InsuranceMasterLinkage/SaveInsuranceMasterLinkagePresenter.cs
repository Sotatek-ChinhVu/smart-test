using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor.InsuranceMasterLinkage;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor.InsuranceMasterLinkage
{
    public class SaveInsuranceMasterLinkagePresenter : ISaveInsuranceMasterLinkageOutputPort
    {
        public Response<SaveInsuranceMasterLinkageResponse> Result { get; private set; } = new();

        public void Complete(SaveInsuranceMasterLinkageOutputData outputData)
        {
            Result.Data = new SaveInsuranceMasterLinkageResponse(outputData.Status == SaveInsuranceMasterLinkageStatus.Success);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(SaveInsuranceMasterLinkageStatus status) => status switch
        {
            SaveInsuranceMasterLinkageStatus.Success => ResponseMessage.Success,
            SaveInsuranceMasterLinkageStatus.Failed => ResponseMessage.Failed,
            SaveInsuranceMasterLinkageStatus.InputDataNull => ResponseMessage.InputDataNull,
            SaveInsuranceMasterLinkageStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveInsuranceMasterLinkageStatus.InvalidDigit1 => ResponseMessage.InvalidDigit1,
            SaveInsuranceMasterLinkageStatus.InvalidDigit2 => ResponseMessage.InvalidDigit2,
            SaveInsuranceMasterLinkageStatus.InvalidDigit3 => ResponseMessage.InvalidDigit3,
            SaveInsuranceMasterLinkageStatus.InvalidDigit4 => ResponseMessage.InvalidDigit4,
            SaveInsuranceMasterLinkageStatus.InvalidDigit5 => ResponseMessage.InvalidDigit5,
            SaveInsuranceMasterLinkageStatus.InvalidDigit6 => ResponseMessage.InvalidDigit6,
            SaveInsuranceMasterLinkageStatus.InvalidDigit7 => ResponseMessage.InvalidDigit7,
            SaveInsuranceMasterLinkageStatus.InvalidDigit8 => ResponseMessage.InvalidDigit8,
            SaveInsuranceMasterLinkageStatus.InvalidHokenNo => ResponseMessage.InvalidHokenNo,
            _ => string.Empty,
        };
    }
}
