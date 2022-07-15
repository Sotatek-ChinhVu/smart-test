using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using UseCase.PatientInformation.GetById;
using UseCase.PatientInformation.GetList;

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
                Status = outputData.PatientInfor == null ? 404 : 1,
                Message = outputData.PatientInfor == null ? ResponseMessage.NotFoundData : ResponseMessage.GetPatientByIdSuccessed
            };
        }
    }
}
