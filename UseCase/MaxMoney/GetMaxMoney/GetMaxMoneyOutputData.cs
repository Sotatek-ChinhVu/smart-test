using Domain.Models.MaxMoney;
using UseCase.Core.Sync.Core;

namespace UseCase.MaxMoney.GetMaxMoney
{
    public class GetMaxMoneyOutputData : IOutputData
    {
        public MaxMoneyModel? Data { get; private set; }
        public GetMaxMoneyStatus Status { get; private set; }

        public GetMaxMoneyOutputData(MaxMoneyModel? data, GetMaxMoneyStatus status)
        {
            Data = data;
            Status = status;
        }
    }
}
