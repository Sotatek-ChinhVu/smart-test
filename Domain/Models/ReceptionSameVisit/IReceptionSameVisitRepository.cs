using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionSameVisit
{
    public interface IReceptionSameVisitRepository
    {
        public IEnumerable<ReceptionSameVisitModel> GetReceptionSameVisit(int hpId, long ptId, int sinDate, int userIdDoctor);
    }
}
