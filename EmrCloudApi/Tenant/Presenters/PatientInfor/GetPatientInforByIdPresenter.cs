using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using UseCase.PatientInformation.GetById;

namespace EmrCloudApi.Tenant.Presenters.PatientInformation
{
    public class GetPatientInforByIdPresenter : IGetPatientInforByIdOutputPort
    {
        public Response<GetPatientInforByIdResponse> Result { get; private set; } = default!;

        public void Complete(GetPatientInforByIdOutputData outputData)
        {
            Result = new Response<GetPatientInforByIdResponse>
            {
                Data = new GetPatientInforByIdResponse()
                {
                    Data = outputData.PatientInfor
                },
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetPatientInforByIdStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetPatientInforByIdStatus.DataNotExist:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetPatientInforByIdStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetPatientInforByIdStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetPatientInforByIdStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetPatientInforByIdStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
            }

        }
    }
}