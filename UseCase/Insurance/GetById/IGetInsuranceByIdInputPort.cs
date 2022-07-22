﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetById
{
    public interface IGetInsuranceByIdInputPort : IInputPort<GetInsuranceByIdInputData, GetInsuranceByIdOutputData>
    {
    }
}
