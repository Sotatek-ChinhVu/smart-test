using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using UseCase.PatientInfor.SearchAdvanced;

namespace EmrCloudApi.Tenant.Presenters.PatientInformation;

public class SearchPatientInfoAdvancedPresenter : ISearchPatientInfoAdvancedOutputPort
{
    public Response<SearchPatientInfoAdvancedResponse> Result { get; private set; } = new Response<SearchPatientInfoAdvancedResponse>();

    public void Complete(SearchPatientInfoAdvancedOutputData output)
    {
        Result.Data = new SearchPatientInfoAdvancedResponse(output.PatientInfos);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SearchPatientInfoAdvancedStatus status) => status switch
    {
        SearchPatientInfoAdvancedStatus.Success => ResponseMessage.Success,
        SearchPatientInfoAdvancedStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}
