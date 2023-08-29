using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.GetListHokenSelect;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetListHokenSelectPresenter : IGetListHokenSelectOutputPort
    {
        public Response<GetListHokenSelectResponse> Result { get; private set; } = new();
        public void Complete(GetListHokenSelectOutputData outputData)
        {
            Result.Data = new GetListHokenSelectResponse(outputData.HokenSelects);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            GetListHokenSelectStatus.Success => ResponseMessage.Success,
            GetListHokenSelectStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
