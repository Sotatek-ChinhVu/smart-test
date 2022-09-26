﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinKubun;
using UseCase.RaiinKubunMst.LoadData;

namespace EmrCloudApi.Tenant.Presenters.RaiinKubun
{
    public class LoadDataKubunSettingPresenter : ILoadDataKubunSettingOutputPort
    {
        public Response<LoadDataKubunSettingResponse> Result { get; private set; } = default!;

        public void Complete(LoadDataKubunSettingOutputData outputData)
        {
            Result = new Response<LoadDataKubunSettingResponse>()
            {
                Data = new LoadDataKubunSettingResponse(outputData.RaiinKubunList),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case LoadDataKubunSettingStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case LoadDataKubunSettingStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
