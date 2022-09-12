using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionVisitingModel
{
    public interface IReceptionVisitingRepository
    {
        IEnumerable<ReceptionVisitingModel> GetReceptionVisiting(long raiinNo);
    }
}
