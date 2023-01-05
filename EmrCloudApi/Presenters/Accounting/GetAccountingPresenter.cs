using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetAccountingPresenter : IGetAccountingOutputPort
    {
        public Response<GetAccountingResponse> Result { get; private set; }
        public void Complete(GetAccountingOutputData outputData)
        {
            Res
        }
    }
}
