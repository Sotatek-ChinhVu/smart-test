using Domain.CommonObject;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Diseases
{
    public interface IPtDiseaseRepository
    {
        IEnumerable<PtDisease> GetAllDiseaseInMonth(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom);
    }
}
