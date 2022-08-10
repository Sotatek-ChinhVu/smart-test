using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.M12FoodAlrgyKbn
{
    public class M12FoodAlrgyKbnModel
    {
        public M12FoodAlrgyKbnModel(string foodKbn, string foodName)
        {
            FoodKbn = foodKbn;
            FoodName = foodName;
        }

        public string FoodKbn { get; private set; }
        public string FoodName { get; private set; }
    }
}
