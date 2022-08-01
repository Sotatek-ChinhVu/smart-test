using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetReceptionListResponse
{
    public GetReceptionListResponse(List<ReceptionRowModel> receptionInfos)
    {
        ReceptionInfos = receptionInfos;
    }
    
    public List<ReceptionRowModel> ReceptionInfos { get; private set; }
}
