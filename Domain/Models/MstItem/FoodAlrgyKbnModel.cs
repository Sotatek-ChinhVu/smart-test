namespace Domain.Models.MstItem
{
    public class FoodAlrgyKbnModel
    {
        public FoodAlrgyKbnModel(string foodKbn, string foodName)
        {
            FoodKbn = foodKbn;
            FoodName = foodName;
            IsDrugAdditives = int.TryParse(foodKbn, out int i) && int.Parse(foodKbn) > 50;
        }

        public string FoodKbn { get; private set; }
        public string FoodName { get; private set; }
        public bool IsDrugAdditives { get; private set; }
    }
}
