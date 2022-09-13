using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.SearchOTC;

namespace EmrCloudApi.Tenant.Presenters.MstItem
{
    public class SearchOTCPresenter : ISearchOTCOutputPort
    {
        public Response<SearchOTCResponse> Result { get; private set; } = default!;

        public void Complete(SearchOTCOutputData outputData)
        {
            Result = new Response<SearchOTCResponse>
            {
                Data = new SearchOTCResponse(outputData.SearchOTCResponse,outputData.Total),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchOTCStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchOTCStatus.Fail:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case SearchOTCStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
