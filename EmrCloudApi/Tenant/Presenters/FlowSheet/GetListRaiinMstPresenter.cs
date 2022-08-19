using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.FlowSheet;
using UseCase.FlowSheet.GetList;

namespace EmrCloudApi.Tenant.Presenters.FlowSheet
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
                    ListRaiinListMstModels = outputData.ListRaiinListMstModels
                },
                Message = ResponseMessage.Success,
                Status = 1
            };
        }
    }
}
