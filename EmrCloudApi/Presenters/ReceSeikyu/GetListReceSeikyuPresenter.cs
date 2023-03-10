using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceSeikyu;
using UseCase.ReceSeikyu.GetList;

namespace EmrCloudApi.Presenters.ReceSeikyu
{
    public class GetListReceSeikyuPresenter : IGetListReceSeikyuOutputPort
    {
        public Response<GetListReceSeikyuResponse> Result { get; private set; } = new Response<GetListReceSeikyuResponse>();

        public void Complete(GetListReceSeikyuOutputData output)
        {
            Result.Data = new GetListReceSeikyuResponse(output.Datas);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetListReceSeikyuStatus status) => status switch
        {
            GetListReceSeikyuStatus.Successful => ResponseMessage.Success,
            GetListReceSeikyuStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetListReceSeikyuStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
