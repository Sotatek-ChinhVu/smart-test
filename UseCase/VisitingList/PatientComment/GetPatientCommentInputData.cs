using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.PatientComment
{
    public class GetPatientCommentInputData:IInputData<GetPatientCommentOutputData>
    {
        public GetPatientCommentInputData(int hpId,long pdId)
        {
            HpId = hpId;
            PdId = pdId;
        }

        public int HpId { get; private set; }
        public long PdId { get; private set; }
    }
}
