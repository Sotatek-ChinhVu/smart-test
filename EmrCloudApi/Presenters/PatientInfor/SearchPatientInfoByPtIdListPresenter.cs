using EmrCloudApi.Constants;
using EmrCloudApi.Responses.PatientInfor;
using EmrCloudApi.Responses;
using UseCase.PatientInfor.SearchPatientInfoByPtIdList;

namespace EmrCloudApi.Presenters.PatientInfor;

public class SearchPatientInfoByPtIdListPresenter : ISearchPatientInfoByPtIdListOutputPort
{
    public Response<SearchPatientInfoByPtIdListResponse> Result { get; private set; } = default!;

    public void Complete(SearchPatientInfoByPtIdListOutputData outputData)
    {
        Result = new Response<SearchPatientInfoByPtIdListResponse>()
        {
            Data = new SearchPatientInfoByPtIdListResponse(outputData.PtInfList.Select(item => new PatientInfoDto(item)).ToList()),
            Status = (int)outputData.Status
        };
        switch (outputData.Status)
        {
            case SearchPatientInfoByPtIdListStatus.Success:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
