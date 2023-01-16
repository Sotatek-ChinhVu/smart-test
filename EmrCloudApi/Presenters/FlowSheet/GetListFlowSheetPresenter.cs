using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.FlowSheet;
using UseCase.FlowSheet.GetList;

namespace EmrCloudApi.Presenters.FlowSheet
{
    public class GetListFlowSheetPresenter : IGetListFlowSheetOutputPort
    {
        public Response<GetListFlowSheetResponse> Result { get; private set; } = new Response<GetListFlowSheetResponse>();
        public void Complete(GetListFlowSheetOutputData outputData)
        {
            Result = new Response<GetListFlowSheetResponse>()
            {
                Data = new GetListFlowSheetResponse(outputData.ListFlowSheetModel, outputData.ListRaiinListMstModel, outputData.ListRaiinListInfModel, outputData.TotalListFlowSheet),
                Message = ResponseMessage.Success,
                Status = 1
            };
        }
    }
}
