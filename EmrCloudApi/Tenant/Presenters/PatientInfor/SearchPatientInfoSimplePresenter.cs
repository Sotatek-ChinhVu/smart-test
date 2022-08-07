using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;

namespace EmrCloudApi.Tenant.Presenters.PatientInformation
{
    public class SearchPatientInfoSimplePresenter : ISearchPatientInfoSimpleOutputPort
    {
        public Response<SearchPatientInforSimpleResponse> Result { get; private set; } = default!;

        public void Complete(SearchPatientInfoSimpleOutputData outputData)
        {
            Result = new Response<SearchPatientInforSimpleResponse>()
            {
                Data = new SearchPatientInforSimpleResponse(outputData.PatientInfoList),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchPatientInfoSimpleStatus.InvalidKeyword:
                    Result.Message = ResponseMessage.InvalidKeyword;
                    break;
                case SearchPatientInfoSimpleStatus.NotFound:
                    Result.Message = ResponseMessage.NotFound;
                    break;
                case SearchPatientInfoSimpleStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}