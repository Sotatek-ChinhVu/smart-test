using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTenItemCds;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTenItemCdsPresenter : IGetTenItemCdsOutputPort
    {
        public Response<GetTenItemCdsResponse> Result { get; private set; } = default!;

        public void Complete(GetTenItemCdsOutputData outputData)
        {
            Result = new Response<GetTenItemCdsResponse>()
            {
                Data = new GetTenItemCdsResponse(outputData.Datas),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTenItemCdsStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetTenItemCdsStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
