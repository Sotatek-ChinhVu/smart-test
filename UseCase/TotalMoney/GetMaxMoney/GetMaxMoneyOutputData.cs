using System;
using UseCase.Core.Sync.Core;

namespace UseCase.TotalMoney.GetMaxMoney
{
    public class GetMaxMoneyOutputData : IOutputData
    {
        public GetMaxMoneyStatus Status { get; private set; }

        public GetMaxMoneyOutputData(GetMaxMoneyStatus status)
        {
            Status = status;
        }
    }
}
