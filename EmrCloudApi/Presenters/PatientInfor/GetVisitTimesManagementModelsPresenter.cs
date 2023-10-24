using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using EmrCloudApi.Responses.PatientInfor.Dto;
using UseCase.PatientInfor.GetVisitTimesManagementModels;

namespace EmrCloudApi.Presenters.PatientInfor;

public class GetVisitTimesManagementModelsPresenter : IGetVisitTimesManagementModelsOutputPort
{
    public Response<GetVisitTimesManagementModelsResponse> Result { get; private set; } = new();

    public void Complete(GetVisitTimesManagementModelsOutputData outputData)
    {
        Result.Data = new GetVisitTimesManagementModelsResponse(outputData.VisitTimesManagementList.Select(item => new VisitTimesManagementDto(item)).ToList());
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetVisitTimesManagementModelsStatus status) => status switch
    {
        GetVisitTimesManagementModelsStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}