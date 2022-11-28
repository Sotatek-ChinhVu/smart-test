using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetFoodAlrgyMasterDataResponse
    {
        public List<FoodAlrgyKbnModel> FoodAlrgyKbnModels { get; set; }

        public GetFoodAlrgyMasterDataResponse()
        {
            FoodAlrgyKbnModels = new List<FoodAlrgyKbnModel>();
        }

        public GetFoodAlrgyMasterDataResponse(List<FoodAlrgyKbnModel> foodAlrgyKbnModels)
        {
            FoodAlrgyKbnModels = foodAlrgyKbnModels;
        }
    }
}
