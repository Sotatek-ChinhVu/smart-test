using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using UseCase.MstItem.GetFoodAlrgy;

namespace EmrCloudApi.Tenant.Presenters.SpecialNote
{
    public class FoodAlrgyMasterDataPresenter : IGetFoodAlrgyOutputPort
    {
        public Response<GetFoodAlrgyMasterDataResponse> Result { get; private set; } = default!;
        public void Complete(GetFoodAlrgyOutputData outputData)
        {
            Result = new Response<GetFoodAlrgyMasterDataResponse>()
            {
                Data = new GetFoodAlrgyMasterDataResponse(),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetFoodAlrgyStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetFoodAlrgyStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    Result.Data = new GetFoodAlrgyMasterDataResponse(outputData.FoodAlrgies);
                    break;
            }
        }
    }
}
