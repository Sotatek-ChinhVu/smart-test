using Domain.Models.RaiinCmtInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinCmtInf
{
    public class GetRaiinCmtInfOutputData : IOutputData
    {
        public GetRaiinCmtInfOutputData(RaiinCmtInfModel raiinCmtInfModel)
        {
            RaiinCmtInfModel = raiinCmtInfModel;
        }

        public RaiinCmtInfModel RaiinCmtInfModel { get; set; }


    }
}
