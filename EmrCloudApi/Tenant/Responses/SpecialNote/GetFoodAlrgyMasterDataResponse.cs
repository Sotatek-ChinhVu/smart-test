using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.SpecialNote
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
