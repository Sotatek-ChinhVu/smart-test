﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.Get;

namespace EmrCloudApi.Tenant.Presenters.Reception
{
    public class GetReceptionPresenter : IGetReceptionOutputPort
    {
        public Response<GetReceptionResponse> Result { get; private set; } = default!;
        
        public void Complete(GetReceptionOutputData outputData)
        {
            Result = new Response<GetReceptionResponse>()
            {
                Data = new GetReceptionResponse(),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetReceptionStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.GetReceptionInvalidRaiinNo;
                    Result.Data.Reception = null;
                    break;
                case GetReceptionStatus.ReceptionNotExisted:
                    Result.Message = ResponseMessage.GetReceptionNotExisted;
                    Result.Data.Reception = null;
                    break;
                case GetReceptionStatus.Successed:
                    Result.Message = ResponseMessage.GetReceptionSuccessed;
                    Result.Data.Reception = outputData.ReceptionModel!.ToDto();
                    break;
            }
        }
    }
}
