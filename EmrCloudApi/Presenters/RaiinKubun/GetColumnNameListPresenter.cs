using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.RaiinKubun;
using UseCase.RaiinKubunMst.GetListColumnName;

namespace EmrCloudApi.Presenters.RaiinKubun
{
    public class GetColumnNameListPresenter : IGetColumnNameListOutputPort
    {
        public Response<GetColumnNameListResponse> Result { get; private set; } = default!;
        public void Complete(GetColumnNameListOutputData outputData)
        {
            Result = new Response<GetColumnNameListResponse>()
            {
                Data = new GetColumnNameListResponse(outputData.ColumnNames.Select(c => new GetColumnNameItem(c.Item1, c.Item2)).ToList()),
                Message = ResponseMessage.Success,
                Status = (byte)outputData.Status
            };
        }
    }
}
