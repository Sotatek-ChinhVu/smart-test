using Domain.Models.ReceptionSameVisit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceptionSameVisit.Get
{
    public class GetReceptionSameVisitOutputData : IOutputData
    {
        public List<ReceptionSameVisitModel> ListSameVisit { get; private set; }

        public GetReceptionSameVisitStatus Status { get; private set; }

        public GetReceptionSameVisitOutputData(List<ReceptionSameVisitModel> listSameVisit, GetReceptionSameVisitStatus status)
        {
            ListSameVisit = listSameVisit;
            Status = status;
        }
    }
}
