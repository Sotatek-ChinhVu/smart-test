using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Byomei;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.Byomei.DiseaseSearch;

namespace EmrCloudApi.Tenant.Presenters.Byomei;

public class DiseaseSearchPresenter : IDiseaseSearchOutputPort
{
    public Response<DiseaseSearchResponse> Result { get; private set; } = new();

    public void Complete(DiseaseSearchOutputData output)
    {
        Result.Data = new DiseaseSearchResponse(output.ListData);
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
