using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetKbnMst;
using UseCase.SetKbnMst.GetSetKbnMstListByGenerationId;

namespace EmrCloudApi.Presenters.SetKbnMst;

public class GetSetKbnMstListByGenerationIdPresenter : IGetSetKbnMstListByGenerationIdOutputPort
{
    public Response<GetSetKbnMstListByGenerationIdResponse> Result { get; private set; } = new();

    public void Complete(GetSetKbnMstListByGenerationIdOutputData outputData)
    {
        Result.Data = new GetSetKbnMstListByGenerationIdResponse(outputData.SetKbnMstList.Select(item => new SetKbnMstDto(item)).ToList());
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetSetKbnMstListByGenerationIdStatus status) => status switch
    {
        GetSetKbnMstListByGenerationIdStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
