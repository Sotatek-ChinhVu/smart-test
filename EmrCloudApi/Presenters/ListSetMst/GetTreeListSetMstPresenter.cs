﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.ListSetMst.GetTreeListSet;
using EmrCloudApi.Responses.ListSetMst;

namespace EmrCloudApi.Presenters.ListSetMst
{
    public class GetTreeListSetMstPresenter : IGetTreeListSetOutputPort
    {
        public Response<GetTreeListSetMstResponse> Result { get; private set; } = default!;

        public void Complete(GetTreeListSetOutputData outputData)
        {
            Result = new Response<GetTreeListSetMstResponse>
            {
                Data = new GetTreeListSetMstResponse()
                {
                    Data = outputData.TreeListSet
                },
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTreeListSetStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;

                case GetTreeListSetStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;

                case GetTreeListSetStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;

                case GetTreeListSetStatus.InvalidKouiKbn:
                    Result.Message = ResponseMessage.InvalidUsageKbn;
                    break;

                case GetTreeListSetStatus.DataNotFound:
                    Result.Message = ResponseMessage.NotFound;
                    break;
            }
        }
    }
}
