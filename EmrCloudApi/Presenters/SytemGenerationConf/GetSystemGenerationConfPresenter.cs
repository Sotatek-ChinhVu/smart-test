﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemGenerationConf;
using UseCase.SystemConf;
using UseCase.SystemGenerationConf.Get;

namespace EmrCloudApi.Presenters.SytemGenerationConf
{
    public class GetSystemGenerationConfPresenter : IGetSystemGenerationConfOutputPort
    {
        public Response<GetSystemGenerationConfResponse> Result { get; private set; } = default!;

        public void Complete(GetSystemGenerationConfOutputData outputData)
        {
            Result = new Response<GetSystemGenerationConfResponse>()
            {
                Data = new GetSystemGenerationConfResponse(outputData.Value, outputData.Param),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSystemGenerationConfStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSystemGenerationConfStatus.InvalidGrpCd:
                    Result.Message = ResponseMessage.InvalidGrpCd;
                    break;
                case GetSystemGenerationConfStatus.InvalidGrpEdaNo:
                    Result.Message = ResponseMessage.InvalidGrpEdaNo;
                    break;
                case GetSystemGenerationConfStatus.InvalidPresentDate:
                    Result.Message = ResponseMessage.InvalidPresentDate;
                    break;
                case GetSystemGenerationConfStatus.InvalidDefaultValue:
                    Result.Message = ResponseMessage.InvalidDefaultValue;
                    break;
                case GetSystemGenerationConfStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetSystemGenerationConfStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
