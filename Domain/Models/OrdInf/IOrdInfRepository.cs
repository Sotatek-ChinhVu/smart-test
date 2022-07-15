using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonObject;

namespace Domain.Models.OrdInfs
{
    public interface IOrdInfRepository
    {
        void Create(OrdInfMst user);

        OrdInfMst Read(OrderId ordId);

        void Update(OrdInfMst ord);

        void Delete(OrderId ordId);

        IEnumerable<OrdInfMst> GetAll();
        IEnumerable<OrdInfMst> GetList(PtId ptId, RaiinNo raiinNo, SinDate sinDate);
        int MaxUserId();
    }
}
