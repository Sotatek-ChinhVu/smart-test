using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionVisitingModel
{
    public interface IReceptionVisitingRepository
    {
        public IEnumerable<ReceptionVisitingModel> GetReceptionVisiting(int hpId, long ptId, int sinDate);
    }
}
