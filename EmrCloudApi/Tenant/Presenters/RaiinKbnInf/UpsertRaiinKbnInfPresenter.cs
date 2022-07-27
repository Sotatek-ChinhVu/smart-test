using EmrCloudApi.Tenant.Responses;
using UseCase.RaiinKbnInf.Upsert;

namespace EmrCloudApi.Tenant.Presenters.RaiinKbnInf;

public class UpsertRaiinKbnInfPresenter : IUpsertRaiinKbnInfOutputPort
{
    public Response<bool> Result { get; set; } = new Response<bool>();

    public void Complete(UpsertRaiinKbnInfOutputData ouput)
    {
        Result.Data = ouput.Success;
        Result.Message = ouput.Message;
        Result.Status = ouput.Status;
    }
}
