using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SpecialNote
{
    public interface ISpecialNoteRepository
    {
        public IEnumerable<SpecialNoteDTO> GetAll();

        public SpecialNoteDTO Get(int ptId, int sinDate);
    }
}
