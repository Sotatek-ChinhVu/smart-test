using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.FlowSheet;
using UseCase.FlowSheet.Upsert;

namespace EmrCloudApi.Presenters.FlowSheet
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
            UpsertFlowSheetStatus.PtIdNoExist => ResponseMessage.UpsertFlowSheetPtIdNoExist,
            UpsertFlowSheetStatus.RaiinNoExist => ResponseMessage.UpsertFlowSheetRainNoNoExist,
            _ => string.Empty
        };
    }
}
