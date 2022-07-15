using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using UseCase.PatientInformation.GetList;

namespace EmrCloudApi.Tenant.Presenters.PatientInformation
{
    public class GetAllPatientInforPresenter: IGetAllPateintInforOutputPort
    {
        public Response<GetAllPatientInforResponse> Result { get; private set; } = default!;

        public void Complete(GetAllOutputData outputData)
        {
            Result = new Response<GetAllPatientInforResponse>
            {
                Data = new GetAllPatientInforResponse()
                {
                    ListData = outputData.ListPatientInfor
                },
                Status = 1,
                Message = ResponseMessage.GetPatientInforListSuccessed
            };
        }
    }
}
