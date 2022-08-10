using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceptionSameVisit.Get
{
    public class GetReceptionSameVisitInputData : IInputData<GetReceptionSameVisitOutputData>
    {
        public GetReceptionSameVisitInputData(int hpId, long ptId, int sinDate, int userIdDoctor)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            UserIdDoctor = userIdDoctor;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int UserIdDoctor { get; private set; }

    }
}
