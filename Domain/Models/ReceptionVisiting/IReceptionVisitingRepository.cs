using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionVisitingModel
{
    public interface IReceptionVisitingRepository
    {
        List<ReceptionVisitingModel> GetReceptionVisiting(long raiinNo);
    }
}
