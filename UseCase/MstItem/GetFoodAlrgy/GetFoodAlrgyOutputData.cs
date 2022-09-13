using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetFoodAlrgy
{
    public class GetFoodAlrgyOutputData : IOutputData
    {
        public GetFoodAlrgyOutputData(List<FoodAlrgyKbnModel> foodAlrgys, GetFoodAlrgyStatus status)
        {
            FoodAlrgies = foodAlrgys;
            Status = status;
        }

        public GetFoodAlrgyOutputData(GetFoodAlrgyStatus status)
        {
            FoodAlrgies = new List<FoodAlrgyKbnModel>();
            Status = status;
        }

        public GetFoodAlrgyStatus Status { get; private set; }
        public List<FoodAlrgyKbnModel> FoodAlrgies { get; private set; }
    }

}
