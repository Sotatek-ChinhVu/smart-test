using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientInfor.GetPtInfModelsByRefNo;

namespace EmrCloudApi.Presenters.PatientInfor;

public class GetPtInfModelsByRefNoPresenter : IGetPtInfModelsByRefNoOutputPort
{
    public Response<GetPtInfModelsByRefNoResponse> Result { get; private set; } = default!;

    public void Complete(GetPtInfModelsByRefNoOutputData outputData)
    {
        Result = new Response<GetPtInfModelsByRefNoResponse>()
        {
            Data = new GetPtInfModelsByRefNoResponse(outputData.PatientInfoList.Select(item => new PatientInfoDto(item)).ToList()),
            Status = (int)outputData.Status
        };
        switch (outputData.Status)
        {
            case GetPtInfModelsByRefNoStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
