﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInformation.GetList
{
    public interface IGetAllPateintInforInputPort : IInputPort<GetAllInputData, GetAllOutputData>
    {
    }
}