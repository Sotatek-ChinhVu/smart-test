using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor.InsuranceMasterLinkage;
using UseCase.PatientInfor.GetInsuranceMasterLinkage;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor.InsuranceMasterLinkage
{
    public class GetInsuranceMasterLinkagePresenter : IGetInsuranceMasterLinkageOutputPort
    {
        public Response<GetInsuranceMasterLinkageResponse> Result { get; private set; } = new Response<GetInsuranceMasterLinkageResponse>();

        public void Complete(GetInsuranceMasterLinkageOutputData outputData)
        {
            Result.Data = new GetInsuranceMasterLinkageResponse(outputData.DefHokenNoModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetInsuranceMasterLinkageStatus status) => status switch
        {
            GetInsuranceMasterLinkageStatus.Success => ResponseMessage.Success,
            GetInsuranceMasterLinkageStatus.Failed => ResponseMessage.Failed,
            GetInsuranceMasterLinkageStatus.NoData => ResponseMessage.NoData,
            GetInsuranceMasterLinkageStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetInsuranceMasterLinkageStatus.InvalidFutansyaNo => ResponseMessage.InvalidFutansyaNo,
            _ => string.Empty
        };
    }
}
