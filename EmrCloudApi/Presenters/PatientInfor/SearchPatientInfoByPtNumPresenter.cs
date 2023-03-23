using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientInfor.SearchPatientInfoByPtNum;

namespace EmrCloudApi.Presenters.PatientInfor;

public class SearchPatientInfoByPtNumPresenter : ISearchPatientInfoByPtNumOutputPort
{
    public Response<SearchPatientInfoByPtNumResponse> Result { get; private set; } = default!;

    public void Complete(SearchPatientInfoByPtNumOutputData outputData)
    {
        Result = new Response<SearchPatientInfoByPtNumResponse>()
        {
            Data = new SearchPatientInfoByPtNumResponse(outputData.PatientInfo),
            Status = (int)outputData.Status
        };
        switch (outputData.Status)
        {
            case SearchPatientInfoByPtNumStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
