using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.IsuranceInfor;
using UseCase.InsuranceInfor.Get;

namespace EmrCloudApi.Tenant.Presenters.InsuranceInfor
{
    public class GetInsuranceInforPresenter: IGetInsuranceInforOutputPort
    {
        public Response<GetInsuranceInforResponse> Result { get; private set; } = default!;
        public void Complete(GetInsuranceInforOutputData output)
        {
            Result = new Response<GetInsuranceInforResponse>()
            {
                Data = new GetInsuranceInforResponse()
                {
                    InsuranceInfor = output.InsuranceInfor
                },
                Status = (byte)output.Status
            };
            switch (output.Status)
            {
                case GetInsuranceInforStatus.DataNotExist:
                    Result.Message = ResponseMessage.DataNotExist;
                    break;
                case GetInsuranceInforStatus.Successed:
                    Result.Message = ResponseMessage.GetPatientInforListSuccessed;
                    break;
                case GetInsuranceInforStatus.PtIdInvalid:
                    Result.Message = ResponseMessage.GetInsuranceInforInvalidPtId;
                    break;
                case GetInsuranceInforStatus.HokenIdInvalid:
                    Result.Message = ResponseMessage.GetInsuranceInforInvalidHokenId;
                    break;
            }
        }
    }
}
