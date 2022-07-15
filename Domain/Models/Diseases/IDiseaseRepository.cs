using Domain.CommonObject;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Diseases
{
    public interface IDiseaseRepository
    {
        IEnumerable<Disease> GetAll(HpId hpId, PtId ptId, SinDate sinDate, DiseaseViewType openFrom);
    }
}
