using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class SyohinModel
    {
        public SyohinModel(string productName, string preparationName, string unit, string maker, string vender)
        {
            ProductName = productName;
            PreparationName = preparationName;
            Unit = unit;
            Maker = maker;
            Vender = vender;
        }

        public SyohinModel()
        {
            ProductName = "";
            PreparationName = "";
            Unit = "";
            Maker = "";
            Vender = "";
        }

        public string ProductName { get; set; }
        public string PreparationName { get; set; }
        public string Unit { get; set; }
        public string Maker { get; set; }
        public string Vender { get; set; }
    }
}
