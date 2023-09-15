﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ListSetGenerationMst;
using UseCase.ListSetGenerationMst.GetListSetGenerationMst;

namespace EmrCloudApi.Presenters.ListSetGenerationMst
{
    public class GetListSetGenerationMstPresenter : IGetListSetGenerationMstOutputPort
    {
        public Response<GetListSetGenerationMstResponse> Result { get; private set; } = default!;

        public void Complete(GetListSetGenerationMstOutputData outputData)
        {
            Result = new Response<GetListSetGenerationMstResponse>
            {
                Data = new GetListSetGenerationMstResponse(outputData.ListSetGenerationMsts),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListSetGenerationMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;

                case GetListSetGenerationMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;

                case GetListSetGenerationMstStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
