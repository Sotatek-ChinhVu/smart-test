using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class DrugMenuItemModel
    {
        public string DrugMenuName { get; private set; }

        public string RawDrugMenuName { get; private set; }

        public int Level { get; private set; }

        public int SeqNo { get; private set; }

        public int DbLevel { get; private set; }

        public string MenuName { get; private set; }
    }
}
