using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class MenuInfModel
    {
        public MenuInfModel(string drugMenuName, string rawDrugMenuName, int level, int seqNo, int dbLevel, string menuName, int indexOfChildrens)
        {
            DrugMenuName = drugMenuName;
            RawDrugMenuName = rawDrugMenuName;
            Level = level;
            SeqNo = seqNo;
            DbLevel = dbLevel;
            MenuName = menuName;
            IndexOfChildrens = indexOfChildrens;
        }

        public MenuInfModel()
        {
            DrugMenuName = string.Empty;
            RawDrugMenuName = string.Empty;
            MenuName = string.Empty;
        }

        public string DrugMenuName { get; private set; }

        public string RawDrugMenuName { get; private set; }

        public int Level { get; private set; }

        public int SeqNo { get; private set; }

        public int DbLevel { get; private set; }

        public string MenuName { get; private set; }

        public int IndexOfChildrens { get; set; }
    }
}
