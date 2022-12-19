
using CommonChecker.Models.OrdInf;
using UseCase.Core.Sync.Core;

namespace UseCase.CommonChecker
{
    public class GetOrderCheckerInputData : IInputData<GetOrderCheckerOutputData>
    {
        public GetOrderCheckerInputData(List<OrdInfoModel> currentListOdr, OrdInfoModel listCheckingOrder)
        {
            CurrentListOdr = currentListOdr;
            ListCheckingOrder = listCheckingOrder;
        }

        public List<OrdInfoModel> CurrentListOdr { get; private set; }
        public OrdInfoModel ListCheckingOrder { get; private set; }
    }
}
