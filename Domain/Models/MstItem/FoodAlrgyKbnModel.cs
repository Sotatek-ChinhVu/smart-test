namespace Domain.Models.MstItem
{
    public class FoodAlrgyKbnModel
    {
        public FoodAlrgyKbnModel(string foodKbn, string foodName, bool isDrugAdditives)
        {
            FoodKbn = foodKbn;
            FoodName = foodName;
            IsDrugAdditives = isDrugAdditives;
        }

        public string FoodKbn { get; private set; }
        public string FoodName { get; private set; }
        public bool IsDrugAdditives { get; private set; }
    }
}
