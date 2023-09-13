﻿using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Responses.MstItem;
using UseCase.UpdateKensaMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateKensaStdMstPresenter : IUpdateKensaMstInputPort
    {
        public Response<UpdateKensaStdMstResponse> Result { get; private set; } = default!;

        public void Complete(UpdateKensaMstOutputData outputData)
        {
            Result = new Response<UpdateKensaStdMstResponse>()
            {
                Data = new UpdateKensaMstResponse(outputData.Status == UpdateKensaMstStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(UpdateKensaMstStatus status) => status switch
        {
            UpdateKensaMstStatus.Success => ResponseMessage.Success,
            UpdateKensaMstStatus.InvalidMasterSbt => ResponseMessage.InvalidMasterSbt,
            UpdateKensaMstStatus.InvalidItemCd => ResponseMessage.InvalidItemCd,
            UpdateKensaMstStatus.InvalidMinAge => ResponseMessage.InvalidMinAge,
            UpdateKensaMstStatus.InvalidMaxAge => ResponseMessage.InvalidMaxAge,
            UpdateKensaMstStatus.InvalidCdKbn => ResponseMessage.InvalidCdKbn,
            UpdateKensaMstStatus.InvalidKokuji1 => ResponseMessage.InvalidKokuji1,
            UpdateKensaMstStatus.InvalidKokuji2 => ResponseMessage.InvalidKokuji2,
            _ => string.Empty
        };
    }
}