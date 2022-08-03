using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UketukeSbtDayInf;
using UseCase.UketukeSbtDayInf.Upsert;

namespace EmrCloudApi.Tenant.Presenters.UketukeSbtDayInf;

public class UpsertUketukeSbtDayInfPresenter : IUpsertUketukeSbtDayInfOutputPort
{
    public Response<UpsertUketukeSbtDayInfResponse> Result { get; private set; } = new Response<UpsertUketukeSbtDayInfResponse>();
    
    public void Complete(UpsertUketukeSbtDayInfOutputData output)
    {
        Result.Data = new UpsertUketukeSbtDayInfResponse(output.Status == UpsertUketukeSbtDayInfStatus.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpsertUketukeSbtDayInfStatus status) => status switch
    {
        UpsertUketukeSbtDayInfStatus.Success => ResponseMessage.Success,
        UpsertUketukeSbtDayInfStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
