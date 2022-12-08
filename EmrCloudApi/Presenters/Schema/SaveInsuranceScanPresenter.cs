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
            Result.Data = new SaveImageResponse(outputData.UrlImage);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(SaveInsuranceScanStatus status) => status switch
        {
            SaveInsuranceScanStatus.Successful => ResponseMessage.Success,
            SaveInsuranceScanStatus.Failed => ResponseMessage.Failed,
            SaveInsuranceScanStatus.FailedSaveToDb => ResponseMessage.SaveInsuranceScanFailedSaveToDb,
            SaveInsuranceScanStatus.Exception => ResponseMessage.ExceptionError,
            SaveInsuranceScanStatus.InvalidImageScan => ResponseMessage.InvalidImageScan,
            SaveInsuranceScanStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveInsuranceScanStatus.RemoveOldImageFailed => ResponseMessage.RemoveOldScanImageFailed,
            SaveInsuranceScanStatus.RemoveOldImageSuccessful => ResponseMessage.RemoveOldScanImageSuccessful,
            SaveInsuranceScanStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            SaveInsuranceScanStatus.OldImageNotFound => ResponseMessage.OldScanImageIsNotFound,
            _ => string.Empty
        };
    }
}
