using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public interface IDrugDetailRepository
    {
        public IEnumerable<DrugMenuItemModel> GetDrugMenu(int hpId, int sinDate, string itemCd);

        DrugDetailModel GetDataDrugSeletedTree(int selectedIndexOfMenuLevel, int level, string drugName, string itemCd, string yjCode);
    }
}
