using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugInfor;
using EmrCloudApi.Responses.DrugInfor.Dto;
using UseCase.DrugInfor.GetSinrekiFilterMstList;

namespace EmrCloudApi.Presenters.DrugInfor;

public class GetSinrekiFilterMstListPresenter : IGetSinrekiFilterMstListOutputPort
{
    public Response<GetSinrekiFilterMstListResponse> Result { get; private set; } = new();

    public void Complete(GetSinrekiFilterMstListOutputData output)
    {
        Result.Data = new GetSinrekiFilterMstListResponse(output.SinrekiFilterMstList.Select(item => new SinrekiFilterMstDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetSinrekiFilterMstListStatus status) => status switch
    {
        GetSinrekiFilterMstListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
