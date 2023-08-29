using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientInfor.GetPtInfModelsByName;

namespace EmrCloudApi.Presenters.PatientInfor;

public class GetPtInfModelsByNamePresenter : IGetPtInfModelsByNameOutputPort
{
    public Response<GetPtInfModelsByNameResponse> Result { get; private set; } = default!;

    public void Complete(GetPtInfModelsByNameOutputData outputData)
    {
        Result = new Response<GetPtInfModelsByNameResponse>()
        {
            Data = new GetPtInfModelsByNameResponse(outputData.PatientInfoList.Select(item => new PatientInfoDto(item)).ToList()),
            Status = (int)outputData.Status
        };
        switch (outputData.Status)
        {
            case GetPtInfModelsByNameStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
