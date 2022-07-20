using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RsvInfo
{
    public interface IRsvInfRepository
    {
        RsvInfModel GetRsvInfModel(long ptId, int sinDate);
    }
}
    