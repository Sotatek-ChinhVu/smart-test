using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetUsedKensaItemCds;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetUsedKensaItemCdsPresenter : IGetUsedKensaItemCdsOutputPort
    {

        public Response<GetUsedKensaItemCdsResponse> Result { get; private set; } = default!;

        public void Complete(GetUsedKensaItemCdsOutputData outputData)
        {
            Result = new Response<GetUsedKensaItemCdsResponse>()
            {
                Data = new GetUsedKensaItemCdsResponse(outputData.KensaItemCd),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetUsedKensaItemCdsStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetUsedKensaItemCdsStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
