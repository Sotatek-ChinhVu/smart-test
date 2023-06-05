using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Diseases;
using UseCase.Diseases.GetAllByomeiByPtId;

namespace EmrCloudApi.Presenters.Diseases;

public class GetAllByomeiByPtIdPresenter
{
    public Response<GetAllByomeiByPtIdResponse> Result { get; private set; } = new();

    public void Complete(GetAllByomeiByPtIdOutputData output)
    {
        Result.Data = new GetAllByomeiByPtIdResponse(output.DiseaseList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetAllByomeiByPtIdStatus status) => status switch
    {
        GetAllByomeiByPtIdStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
