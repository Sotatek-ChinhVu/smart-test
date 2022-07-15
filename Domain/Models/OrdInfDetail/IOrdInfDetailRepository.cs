using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfDetails
{
    public interface IOrdInfDetailRepository
    {
        void Create(OrdInfDetailMst ordInfDetail);

        OrdInfDetailMst Read(HpId ordId, RaiinNo raiinNo, RpNo rpNo, RpEdaNo rpEdaNo, RowNo rowNo);

        void Update(OrdInfDetailMst ordInfDetail);

        void Delete(HpId ordId, RaiinNo raiinNo, RpNo rpNo, RpEdaNo rpEdaNo, RowNo rowNo);

        IEnumerable<OrdInfDetailMst> GetAll();
        IEnumerable<OrdInfDetailMst> GetList(PtId ptId, RaiinNo raiinNo, SinDate sinDate);

        int MaxUserId();
    }
}
