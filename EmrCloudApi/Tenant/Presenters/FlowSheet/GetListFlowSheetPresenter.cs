using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.FlowSheet;
using UseCase.FlowSheet.GetList;

namespace EmrCloudApi.Tenant.Presenters.FlowSheet
{
    public class GetListFlowSheetPresenter : IGetListFlowSheetOutputPort
    {
        public Response<GetListFlowSheetResponse> Result { get; private set; } = new Response<GetListFlowSheetResponse>();
        public void Complete(GetListFlowSheetOutputData outputData)
        {
            Result = new Response<GetListFlowSheetResponse>()
            {
                Data = new GetListFlowSheetResponse()
                {
                    ListFlowSheet = outputData.ListFlowSheetModel,
                    ListRaiinListMstModels = outputData.ListRaiinListMstModels,
                    ListHolidayModel = outputData.ListHolidayModel,
                },
                Message = ResponseMessage.Success,
                Status = 1
            };
        }
    }
}
