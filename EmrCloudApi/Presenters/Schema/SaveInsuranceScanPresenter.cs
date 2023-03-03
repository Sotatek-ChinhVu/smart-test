using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Schema;
using EmrCloudApi.Responses;
using Schema.Insurance.SaveInsuranceScan;

namespace EmrCloudApi.Presenters.Schema
{
    public class SaveInsuranceScanPresenter : ISaveInsuranceScanOutputPort
    {
        public Response<SaveImageResponse> Result { get; private set; } = new();

        public void Complete(SaveInsuranceScanOutputData outputData)
        {
            Result.Data = new SaveImageResponse(outputData.Status, outputData.UrlCreateds);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(SaveInsuranceScanStatus status) => status switch
        {
            SaveInsuranceScanStatus.Successful => ResponseMessage.Success,
            SaveInsuranceScanStatus.Error => ResponseMessage.Error,
            SaveInsuranceScanStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveInsuranceScanStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            SaveInsuranceScanStatus.InvalidNoDataSave => ResponseMessage.InvalidNoDataSave,
            _ => string.Empty
        };
    }
}
