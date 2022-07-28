using Domain.Models.CalculationInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.CalculationInf
{
    public class CalculationInfOutputData : IOutputData
    {
        public CalculationInfOutputData(List<CalculationInfModel> listCalculation, CalculationInfStatus status)
        {
            ListCalculation = listCalculation;
            Status = status;
        }
        public List<CalculationInfModel> ListCalculation { get; private set; }
        public CalculationInfStatus Status { get; private set; }
       
    }
}
