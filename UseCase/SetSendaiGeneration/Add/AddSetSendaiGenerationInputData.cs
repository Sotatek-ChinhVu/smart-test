﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SetSendaiGeneration.Add
{
    public class AddSetSendaiGenerationInputData: IInputData<AddSetSendaiGenerationOutputData>
    {
        public AddSetSendaiGenerationInputData(int startDate, int hpId, int userId)
        {
            StartDate = startDate;
            HpId = hpId;
            UserId = userId;
        }

        public int StartDate { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
    }
}
