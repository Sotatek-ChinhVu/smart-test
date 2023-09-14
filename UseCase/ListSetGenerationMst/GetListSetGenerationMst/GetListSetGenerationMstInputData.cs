﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.ListSetGenerationMst.GetListSetGenerationMst
{
    public sealed class GetListSetGenerationMstInputData : IInputData<GetListSetGenerationMstOutputData>
    {
        public GetListSetGenerationMstInputData(int hpId)
        {
            this.hpId = hpId;
        }

        public int hpId { get; private set; }
    }
}
