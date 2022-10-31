using Domain.Models.InsuranceMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.KohiHokenMst.Get
{
    public class GetKohiHokenMstOutputData : IOutputData
    {
        public HokenMstModel HokenMstModel { get; private set; }

        public GetKohiHokenMstStatus Status { get; private set; }

        public GetKohiHokenMstOutputData(HokenMstModel data, GetKohiHokenMstStatus status)
        {
            HokenMstModel = data;
            Status = status;
        }
    }
}
