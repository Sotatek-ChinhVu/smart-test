using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.FlowSheet;
using UseCase.FlowSheet.GetTooltip;

namespace EmrCloudApi.Presenters.FlowSheet
{
    public class GetFlowSheetTooltipPresenter : IGetTooltipOutputPort
    {
        public Response<GetFlowSheetTooltipResponse> Result { get; private set; } = new Response<GetFlowSheetTooltipResponse>();
        public void Complete(GetTooltipOutputData outputData)
        {
            Result = new Response<GetFlowSheetTooltipResponse>()
            {
                Data = new GetFlowSheetTooltipResponse(outputData.Values.Select(o => new GetFlowSheetTooltipItem(o.Item1, o.Item2)).ToList()),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private string GetMessage(GetTooltipStatus status) => status switch
        {
            GetTooltipStatus.Success => ResponseMessage.Success,
            GetTooltipStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            GetTooltipStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetTooltipStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            GetTooltipStatus.InvalidStartDate => ResponseMessage.InvalidStartDate,
            GetTooltipStatus.InvalidEndDate => ResponseMessage.InvalidEndDate,
            _ => string.Empty
        };
    }
}
