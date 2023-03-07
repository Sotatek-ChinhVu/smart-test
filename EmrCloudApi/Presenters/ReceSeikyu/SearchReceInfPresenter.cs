using EmrCloudApi.Constants;
using EmrCloudApi.Responses.ReceSeikyu;
using EmrCloudApi.Responses;
using UseCase.ReceSeikyu.SearchReceInf;

namespace EmrCloudApi.Presenters.ReceSeikyu
{
    public class SearchReceInfPresenter : ISearchReceInfOutputPort
    {
        public Response<SearchReceInfResponse> Result { get; private set; } = new Response<SearchReceInfResponse>();

        public void Complete(SearchReceInfOutputData output)
        {
            Result.Data = new SearchReceInfResponse(output.Data, output.PtName);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(SearchReceInfStatus status) => status switch
        {
            SearchReceInfStatus.Successful => ResponseMessage.Success,
            SearchReceInfStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SearchReceInfStatus.NoData => ResponseMessage.NoData,
            SearchReceInfStatus.InvalidPtNum => ResponseMessage.InvalidPtNum,
            SearchReceInfStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
            _ => string.Empty
        };
    }
}
