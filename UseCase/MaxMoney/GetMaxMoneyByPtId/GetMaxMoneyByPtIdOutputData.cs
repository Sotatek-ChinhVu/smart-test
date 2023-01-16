using Domain.Models.MaxMoney;
using UseCase.Core.Sync.Core;

namespace UseCase.MaxMoney.GetMaxMoneyByPtId
{
    public class GetMaxMoneyByPtIdOutputData : IOutputData
    {
        public GetMaxMoneyByPtIdOutputData(IEnumerable<MaxMoneyModel> datas, GetMaxMoneyByPtIdStatus status )
        {
            Datas = datas;
            Status = status;
        }

        public IEnumerable<MaxMoneyModel> Datas { get; private set; }

        public GetMaxMoneyByPtIdStatus Status { get; private set; }
    }
}
