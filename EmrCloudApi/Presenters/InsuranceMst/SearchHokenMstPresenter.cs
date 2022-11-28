﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceMst;
using UseCase.SearchHokensyaMst.Get;

namespace EmrCloudApi.Presenters.InsuranceMst
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
