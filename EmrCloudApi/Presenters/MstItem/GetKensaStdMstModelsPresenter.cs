using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetKensaStdMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetKensaStdMstModelsPresenter : IGetKensaStdMstOutputPort
    {
        public Response<GetKensaStdMstModelsResponse> Result { get; private set; } = default!;

        public void Complete(GetKensaStdMstOutputData outputData)
        {
            Result = new Response<GetKensaStdMstModelsResponse>()
            {
                Data = new GetKensaStdMstModelsResponse(outputData.KensaStdMsts),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetKensaStdMstIStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetKensaStdMstIStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
