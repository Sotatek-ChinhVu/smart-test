﻿using Domain.Models.CalculationInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CalculationInf
{
    public interface ICalculationInfRepository
    {
        IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId); 
    }
}
