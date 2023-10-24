using EmrCloudApi.Responses.MstItem.DiseaseSearch;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetDiseaseList;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.MstItem;

public class GetDiseaseListPresenter : IGetDiseaseListOutputPort
{
    public Response<DiseaseSearchResponse> Result { get; private set; } = new();

    public void Complete(GetDiseaseListOutputData output)
    {
        Result.Data = new DiseaseSearchResponse(output.ListData.Select(item => new DiseaseSearchModel(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetDiseaseListStatus status) => status switch
    {
        GetDiseaseListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}