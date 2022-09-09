using Domain.Models.SpecialNote.ImportantNote;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Get
{
    public class GetFoodAlrgyOutputData : IOutputData
    {
        public GetFoodAlrgyOutputData(FoodAlrgyMasterData foodAlrgy, GetFoodAlrgyStatus status)
        {
            FoodAlrgy = foodAlrgy;
            Status = status;
        }

        public GetFoodAlrgyOutputData(GetFoodAlrgyStatus status)
        {
            FoodAlrgy = new FoodAlrgyMasterData(new List<FoodAlrgyKbnModel>());
            Status = status;
        }

        public GetFoodAlrgyStatus Status { get; private set; }
        public FoodAlrgyMasterData FoodAlrgy { get; private set; }
    }

}
