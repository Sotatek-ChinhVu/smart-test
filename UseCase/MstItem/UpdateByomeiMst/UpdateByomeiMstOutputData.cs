﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.UpdateAdoptedItemList;

namespace UseCase.MstItem.UpdateByomeiMst
{
    public class UpdateByomeiMstOutputData : IOutputData
    {
        public UpdateByomeiMstStatus Status { get; private set; }
        public bool CheckResult { get; set; }

        public UpdateByomeiMstOutputData(bool checkResult, UpdateByomeiMstStatus status)
        {
            Status = status;
            CheckResult = checkResult;
        }
    }
}
