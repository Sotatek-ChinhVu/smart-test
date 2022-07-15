using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Read
{
    public class GetSpecialNoteInputData : IInputData<GetSpecialNoteOutputData>
    {
        public GetSpecialNoteInputData(PtId ptId, SinDate sinDate)
        {
            PtId = ptId;
            SinDate = sinDate;
        }

        public PtId PtId { get; set; }

        public SinDate SinDate { get; set; }


    }
}
