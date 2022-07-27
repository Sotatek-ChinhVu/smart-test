using Domain.Models.Reception;
using EmrCloudApi.Tenant.Responses;
using UseCase.Reception.GetList;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class GetReceptionListPresenter : IGetReceptionListOutputPort
{
    public Response<List<ReceptionRowModel>> Result { get; set; } = new Response<List<ReceptionRowModel>>();

    public void Complete(GetReceptionListOutputData outputData)
    {
        Result.Data = outputData.Models;
        Result.Message = outputData.Message;
        Result.Status = outputData.Status;
    }
}
