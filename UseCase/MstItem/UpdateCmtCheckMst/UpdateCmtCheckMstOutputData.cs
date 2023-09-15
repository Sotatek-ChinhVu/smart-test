using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.GetCmtCheckMstList;

namespace UseCase.MstItem.UpdateCmtCheckMst
{
    public class UpdateCmtCheckMstOutputData : IOutputData
    {
        public UpdateCmtCheckMstStatus Status { get; private set; }
        public bool CheckResult { get; set; }

        public UpdateCmtCheckMstOutputData(bool checkResult, UpdateCmtCheckMstStatus status)
        {
            Status = status;
            CheckResult = checkResult;
        }

        public enum UpdateCmtCheckMstStatus : byte
        {
            Successed = 1,
            InValidHpId = 2,
            InValidUserId = 3,
            InvalidDataUpdate = 4,
            Failed = 5
        }
    }
}
