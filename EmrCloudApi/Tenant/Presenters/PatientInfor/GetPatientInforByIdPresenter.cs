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
                    Result.Message = ResponseMessage.GetPatientByIdSuccessed;
                    break;
                case GetPatientInforByIdStatus.DataNotExist:
                    Result.Message = ResponseMessage.GetPatientInforNotExist;
                    break;
                case GetPatientInforByIdStatus.InvalidId:
                    Result.Message = ResponseMessage.GetPatientByIdInvalidId;
                    break;
                default:
                    break;
            }

        }
    }
}