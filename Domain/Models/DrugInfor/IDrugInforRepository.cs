using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugInfor
{
    public interface IDrugInforRepository : IRepositoryBase
    {
        DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd);
    }
}
