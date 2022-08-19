﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.FlowSheet;
using UseCase.FlowSheet.Upsert;

namespace EmrCloudApi.Tenant.Presenters.FlowSheet
{
    public class UpsertFlowSheetPresenter : IUpsertFlowSheetOutputPort
    {
        public Response<UpsertFlowSheetResponse> Result { get; private set; } = new Response<UpsertFlowSheetResponse>();
        public void Complete(UpsertFlowSheetOutputData outputData)
        {
            Result = new Response<UpsertFlowSheetResponse>()
            {
                Data = new UpsertFlowSheetResponse(outputData.Status == UpsertFlowSheetStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private string GetMessage(UpsertFlowSheetStatus status) => status switch
        {
            UpsertFlowSheetStatus.Success => ResponseMessage.UpsertFlowSheetSuccess,
            UpsertFlowSheetStatus.UpdateNoSuccess => ResponseMessage.UpsertFlowSheetUpdateNoSuccess,
            UpsertFlowSheetStatus.InputDataNoValid => ResponseMessage.UpsertFlowSheetInputDataNoValid,
            UpsertFlowSheetStatus.RainNoNoValid => ResponseMessage.UpsertFlowSheetInvalidRaiinNo,
            UpsertFlowSheetStatus.PtIdNoValid => ResponseMessage.UpsertFlowSheetInvalidPtId,
            UpsertFlowSheetStatus.SinDateNoValid => ResponseMessage.UpsertFlowSheetInvalidSinDate,
            UpsertFlowSheetStatus.TagNoNoValid => ResponseMessage.UpsertFlowSheetInvalidTagNo,
            UpsertFlowSheetStatus.CmtKbnNoValid => ResponseMessage.UpsertFlowSheetInvalidCmtKbn,
            UpsertFlowSheetStatus.RainListCmtSeqNoNoValid => ResponseMessage.UpsertFlowSheetInvalidRainCmtSeqNo,
            UpsertFlowSheetStatus.RainListTagSeqNoNoValid => ResponseMessage.UpsertFlowSheetInvalidRainListTagSeqNo,
            UpsertFlowSheetStatus.PtIdNoExist => ResponseMessage.UpsertFlowSheetPtIdNoExist,
            UpsertFlowSheetStatus.RaiinNoExist => ResponseMessage.UpsertFlowSheetRainNoNoExist,
            _ => string.Empty
        };
    }
}
