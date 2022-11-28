using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KarteFilter;
using UseCase.KarteFilter.GetListKarteFilter;

namespace EmrCloudApi.Presenters.KarteFilter
{
    public class GetKarteFilterMstPresenter : IGetKarteFilterOutputPort
    {
        public Response<GetKarteFilterMstResponse> Result { get; private set; } = new Response<GetKarteFilterMstResponse>();

        public void Complete(GetKarteFilterOutputData output)
        {
            Result.Data = new GetKarteFilterMstResponse(output.ListData);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetKarteFilterStatus status) => status switch
        {
            GetKarteFilterStatus.Successed => ResponseMessage.Success,
            GetKarteFilterStatus.NoData => ResponseMessage.NoData,
            GetKarteFilterStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            GetKarteFilterStatus.Error => ResponseMessage.NotFound,
            _ => string.Empty
        };
    }
}
