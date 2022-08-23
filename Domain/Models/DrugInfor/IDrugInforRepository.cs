using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugInfor
{
    public interface IDrugInforRepository
    {
        public DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd);
    }
}
