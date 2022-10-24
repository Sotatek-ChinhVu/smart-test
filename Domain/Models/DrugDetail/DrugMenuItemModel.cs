using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class DrugMenuItemModel
    {
        public DrugMenuItemModel(string drugMenuName, string rawDrugMenuName, int level, int seqNo, int dbLevel, string menuName, int indexOfMenuLevel, string yjCode)
        {
            DrugMenuName = drugMenuName;
            RawDrugMenuName = rawDrugMenuName;
            Level = level;
            SeqNo = seqNo;
            DbLevel = dbLevel;
            MenuName = menuName;
            YjCode = yjCode;
            IndexOfMenuLevel = indexOfMenuLevel;
        }

        public DrugMenuItemModel()
        {
            DrugMenuName = string.Empty;
            RawDrugMenuName = string.Empty;
            MenuName = string.Empty;
            YjCode = string.Empty;
        }

        public string DrugMenuName { get; private set; }

        public string RawDrugMenuName { get; private set; }

        public int Level { get; private set; }

        public int SeqNo { get; private set; }

        public int DbLevel { get; private set; }

        public string MenuName { get; private set; }

        public string YjCode { get; private set; }

        public int IndexOfMenuLevel { get; private set; }

        public List<DrugMenuItemModel> Children { get; set; } = new List<DrugMenuItemModel>();
    }
}
