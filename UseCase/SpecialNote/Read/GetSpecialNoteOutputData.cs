using Domain.Models.SpecialNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Read
{
    public class GetSpecialNoteOutputData : IOutputData
    {
        public SpecialNoteDTO SpecialNoteDTO { get; set; }

        public GetSpecialNoteOutputData(SpecialNoteDTO specialNoteDTO)
        {
            SpecialNoteDTO = specialNoteDTO;
        }
    }
}
