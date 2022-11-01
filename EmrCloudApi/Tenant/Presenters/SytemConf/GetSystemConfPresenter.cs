﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SystemConf;
using UseCase.SystemConf;

namespace EmrCloudApi.Tenant.Presenters.SytemConf
{
    public class GetSystemConfPresenter : IGetSystemConfOutputPort
    {
        public Response<GetSystemConfResponse> Result { get; private set; } = default!;

        public void Complete(GetSystemConfOutputData outputData)
        {
            Result = new Response<GetSystemConfResponse>()
            {
                Data = new GetSystemConfResponse(outputData.Model),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSystemConfStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSystemConfStatus.InvalidGrpCd:
                    Result.Message = ResponseMessage.InvalidGrpCd;
                    break;
                case GetSystemConfStatus.InvalidGrpEdaNo:
                    Result.Message = ResponseMessage.InvalidGrpEdaNo;
                    break;
                case GetSystemConfStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetSystemConfStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
