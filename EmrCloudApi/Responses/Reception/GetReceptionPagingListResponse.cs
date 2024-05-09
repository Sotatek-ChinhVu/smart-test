using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception;

public class GetReceptionPagingListResponse
{
    public GetReceptionPagingListResponse(List<ReceptionRowModel> receptionInfos, int totalItems)
    {
        ReceptionInfos = receptionInfos;
        TotalItems = totalItems;
    }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public int TotalItems { get; private set; }
}
