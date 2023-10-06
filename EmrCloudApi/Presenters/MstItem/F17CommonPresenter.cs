using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.IsUsingKensa;

namespace EmrCloudApi.Presenters.MstItem
{
    public class F17CommonPresenter : IF17CommonOutputPort
    {
        public Response<F17CommonResponse> Result { get; private set; } = default!;

        public void Complete(F17CommonOutputData outputData)
        {
            Result = new Response<F17CommonResponse>()
            {
                Data = new F17CommonResponse(outputData.KensaItemCd, outputData.Status, outputData.KensaStdMsts, outputData.ItemCd, outputData.MaterialMsts, outputData.ContainerMsts, outputData.KensaCenterMsts, outputData.TenOfItem, outputData.LatestSedai, outputData.TenItemModels),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case F17CommonStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case F17CommonStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case F17CommonStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
