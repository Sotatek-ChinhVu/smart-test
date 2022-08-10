using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InsuranceMst;
using UseCase.SearchHokensyaMst.Get;

namespace EmrCloudApi.Tenant.Presenters.InsuranceMst
{
    public class SearchHokenMstPresenter : ISearchHokensyaMstOutputPort
    {
        public Response<SearchHokensyaMstResponse> Result { get; private set; } = default!;
        public void Complete(SearchHokensyaMstOutputData output)
        {
            Result = new Response<SearchHokensyaMstResponse>()
            {

                Data = new SearchHokensyaMstResponse(output.ListHokensyaMst),
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {

                case SearchHokensyaMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SearchHokensyaMstStatus.InvalidPageIndex:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SearchHokensyaMstStatus.InvalidPageCount:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case SearchHokensyaMstStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidHokenId;
                    break;
                case SearchHokensyaMstStatus.InvalidKeyword:
                    Result.Message = ResponseMessage.InvalidKeyword;
                    break;
                case SearchHokensyaMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
