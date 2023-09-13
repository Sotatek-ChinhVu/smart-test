﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.SinKoui.GetSinKoui;

namespace UseCase.SetSendaiGeneration.GetList
{
    public class SetSendaiGenerationInputData : IInputData<SetSendaiGenerationOutputData>
    {
        public SetSendaiGenerationInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; set; }
    }
}
