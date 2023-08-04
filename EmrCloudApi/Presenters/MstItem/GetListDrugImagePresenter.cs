using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetListDrugImage;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetListDrugImagePresenter : IGetListDrugImageOutputPort
    {
        public Response<GetListDrugImageResponse> Result { get; private set; } = default!;

        public void Complete(GetListDrugImageOutputData outputData)
        {
            Result = new Response<GetListDrugImageResponse>
            {
                Data = new GetListDrugImageResponse(outputData.ImageList),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListDrugImageStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetListDrugImageStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetListDrugImageStatus.InvalidTypeImage:
                    Result.Message = ResponseMessage.InvalidTypeItem;
                    break;
            }
        }
    }
}
