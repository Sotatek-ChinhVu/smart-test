using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.RsvInf.GetById
{
    public class GetRsvInfByPtIdInputData : IInputData<GetRsvInfByPtIdOutputData>
    {
        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public GetRsvInfByPtIdInputData(long ptId, int sinDate)
        {
            PtId = ptId;

            SinDate = sinDate;
        }
    }
}