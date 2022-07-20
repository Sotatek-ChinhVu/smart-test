using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.KensaInfDetail.GetListByPtIdAndSinDate
{
    public class GetListKensaInfDetailByPtIdAndSinDateInputData : IInputData<GetListKensaInfDetailByPtIdAndSinDateOutputData>
    {
        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public GetListKensaInfDetailByPtIdAndSinDateInputData(long ptId, int sinDate)
        {
            PtId = ptId;
            SinDate = sinDate;
        }
    }
}
