using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem.DiseaseSearch;
using UseCase.MstItem.DiseaseSearch;

namespace EmrCloudApi.Presenters.MstItem;

public class DiseaseSearchPresenter : IDiseaseSearchOutputPort
{
    public Response<DiseaseSearchResponse> Result { get; private set; } = new();

    public void Complete(DiseaseSearchOutputData output)
    {
        Result.Data = new DiseaseSearchResponse(output.ListData.Select(item => new DiseaseSearchModel(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(DiseaseSearchStatus status) => status switch
    {
        DiseaseSearchStatus.Successed => ResponseMessage.Success,
        DiseaseSearchStatus.Failed => ResponseMessage.Failed,
        DiseaseSearchStatus.InvalidPageCount => ResponseMessage.InvalidPageCount,
        DiseaseSearchStatus.InvalidPageIndex => ResponseMessage.InvalidStartIndex,
        _ => string.Empty
    };
}
