using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientInfor.GetPtInfByRefNo;

namespace EmrCloudApi.Presenters.PatientInfor;

public class GetPtInfByRefNoPresenter : IGetPtInfByRefNoOutputPort
{
    public Response<GetPtInfByRefNoResponse> Result { get; private set; } = default!;

    public void Complete(GetPtInfByRefNoOutputData outputData)
    {
        Result = new Response<GetPtInfByRefNoResponse>()
        {
            Data = new GetPtInfByRefNoResponse(new PatientInfoDto(outputData.PatientInfo)),
            Status = (int)outputData.Status
        };
        switch (outputData.Status)
        {
            case GetPtInfByRefNoStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
