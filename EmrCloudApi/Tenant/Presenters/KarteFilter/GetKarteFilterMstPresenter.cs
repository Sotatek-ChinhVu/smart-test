using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteFilter;
using UseCase.KarteFilter.GetListKarteFilter;

namespace EmrCloudApi.Tenant.Presenters.KarteFilter
{
    public class GetKarteFilterMstPresenter : IKarteFilterOutputPort
    {
        public Response<GetKarteFilterMstResponse> Result { get; private set; } = new Response<GetKarteFilterMstResponse>();

        public void Complete(KarteFilterOutputData output)
        {
            Result.Data = new GetKarteFilterMstResponse(output.ListData);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(KarteFilterStatus status) => status switch
        {
            KarteFilterStatus.Successed => ResponseMessage.Success,
            KarteFilterStatus.NoData => ResponseMessage.NoData,
            KarteFilterStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            KarteFilterStatus.Error => ResponseMessage.NotFound,
            _ => string.Empty
        };
    }
}
