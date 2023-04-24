﻿using Domain.Models.MstItem;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.MstItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using Helper.Mapping;
using UseCase.MstItem.GetListTenMstOrigin;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetListTenMstOriginPresenter : IGetListTenMstOriginOutputPort
    {
        public Response<GetListTenMstOriginResponse> Result { get; private set; } = default!;

        public void Complete(GetListTenMstOriginOutputData outputData)
        {
            Result = new Response<GetListTenMstOriginResponse>()
            {
                Data = new GetListTenMstOriginResponse(Mapper.Map<TenMstOriginModel,TenMstOriginModelDto>(outputData.TenMsts), outputData.StartDateDisplay),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListTenMstOriginStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetListTenMstOriginStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetListTenMstOriginStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
