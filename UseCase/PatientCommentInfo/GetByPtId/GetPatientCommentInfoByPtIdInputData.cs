using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientCommentInfo.GetById
{
    public class GetPatientCommentInfoByPtIdInputData : IInputData<GetPatientCommentInfoByPtIdOutputData>
    {
        public long PtId { get; private set; }

        public GetPatientCommentInfoByPtIdInputData(long ptId)
        {
            PtId = ptId;
        }
    }
}