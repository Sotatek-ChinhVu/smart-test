using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.FlowSheet;
using UseCase.FlowSheet.GetList;

namespace EmrCloudApi.Presenters.FlowSheet
{
    public class GetListRaiinMstPresenter : IGetListFlowSheetOutputPort
    {
        public Response<GetListRaiinMstResponse> Result { get; private set; } = new Response<GetListRaiinMstResponse>();
        public void Complete(GetListFlowSheetOutputData outputData)
        {
            Result = new Response<GetListRaiinMstResponse>()
            {
                Data = new GetListRaiinMstResponse()
                {
                    ListRaiinListMstModels = outputData.ListRaiinListMstModel
                },
                Message = ResponseMessage.Success,
                Status = 1
            };
        }
    }
}
