using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.IsuranceMst
{
    public class KantokuMstModel
    {
        public KantokuMstModel(string roudouCD, string kantokuCD, string kantokuName)
        {
            RoudouCD = roudouCD;
            KantokuCD = kantokuCD;
            KantokuName = kantokuName;
        }

        public string RoudouCD { get; private set; }
        public string KantokuCD { get; private set; }
        public string KantokuName { get; private set; }
    }
}
