using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SpecialNote
{
    public class SpecialNoteDTO
    {
        public int PtId { get; set; }

        public string? Comment { get; set; }    

        public string? Tel { get; set; }

        public List<MedicalSchedule>? MedicalSchedule { get; set; } = new List<MedicalSchedule>();

        public string? FamilyHistory { get; set; }

        public string? Address { get; set; }

        public string? BodyIndex { get; set; }
    }
}
