﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetList
{
    public class GetInsuranceListInputData : IInputData<GetInsuranceListByIdOutputData>
    {
        public long PtId { get; private set; }
        public GetInsuranceListInputData(long ptId)
        {
            PtId = ptId;
        }
    }
}