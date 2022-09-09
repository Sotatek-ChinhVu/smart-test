using Helper.Extendsions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Domain.Models.SpecialNote.ImportantNote
{
    public class FoodAlrgyMasterData
    {
        public List<FoodAlrgyKbnModel> FoodAlrgyKbnModels { get; set; }

        public FoodAlrgyMasterData(List<FoodAlrgyKbnModel> foodAlrgyKbnModels)
        {
            FoodAlrgyKbnModels = foodAlrgyKbnModels;
        }
    }
    public class FoodAlrgyKbnModel
    {
        public string FoodKbn { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;
        public bool IsDrugAdditives { get; set; } 
    }
}
