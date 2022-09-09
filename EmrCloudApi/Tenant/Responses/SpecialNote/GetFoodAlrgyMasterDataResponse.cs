using Domain.Models.SpecialNote.ImportantNote;

namespace EmrCloudApi.Tenant.Responses.SpecialNote
{
    public class GetFoodAlrgyMasterDataResponse
    {
        public FoodAlrgyMasterData FoodAlrgyMasterData { get; set; }

        public GetFoodAlrgyMasterDataResponse()
        {
            FoodAlrgyMasterData = new FoodAlrgyMasterData(new List<FoodAlrgyKbnModel>());
        }

        public GetFoodAlrgyMasterDataResponse(FoodAlrgyMasterData foodAlrgyMasterData)
        {
            FoodAlrgyMasterData = foodAlrgyMasterData;
        }
    }
}
