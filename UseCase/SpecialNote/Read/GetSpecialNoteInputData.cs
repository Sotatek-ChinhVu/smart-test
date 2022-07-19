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
        public GetSpecialNoteInputData(int ptId, int sinDate)
        {
            PtId = ptId;
            SinDate = sinDate;
        }

        public int PtId { get; set; }

        public int SinDate { get; set; }


    }
}
