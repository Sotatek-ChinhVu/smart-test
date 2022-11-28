using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.FlowSheet;
using UseCase.FlowSheet.GetList;

namespace EmrCloudApi.Presenters.FlowSheet
{
    public class GetListHolidayPresenter : IGetListFlowSheetOutputPort
    {
        public Response<GetListHolidayResponse> Result { get; private set; } = new Response<GetListHolidayResponse>();
        public void Complete(GetListFlowSheetOutputData outputData)
        {
            Result = new Response<GetListHolidayResponse>()
            {
                Data = new GetListHolidayResponse()
                {
                    ListHolidayModel = outputData.ListHolidayModel
                },
                Message = ResponseMessage.Success,
                Status = 1
            };
        }
    }
}
