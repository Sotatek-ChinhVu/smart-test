using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Schema;
using EmrCloudApi.Responses;
using UseCase.Schema.GetListInsuranceScan;

namespace EmrCloudApi.Presenters.Schema
{
    public class GetListInsuranceScanPresenter : IGetListInsuranceScanOutputPort
    {
        public Response<GetListInsuranceScanResponse> Result { get; private set; } = new();

        public void Complete(GetListInsuranceScanOutputData outputData)
        {
            Result.Data = new GetListInsuranceScanResponse(outputData.InsuranceScans.Select(x=> new InsuranceScanDto(x)).ToList());
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetListInsuranceScanStatus status) => status switch
        {
            GetListInsuranceScanStatus.Successful => ResponseMessage.Success,
            GetListInsuranceScanStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetListInsuranceScanStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            GetListInsuranceScanStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
