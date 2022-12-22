
using CommonChecker.Models.OrdInf;
using UseCase.Core.Sync.Core;

namespace UseCase.CommonChecker
{
    public class GetOrderCheckerInputData : IInputData<GetOrderCheckerOutputData>
    {
        public GetOrderCheckerInputData(List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder)
        {
            CurrentListOdr = currentListOdr;
            ListCheckingOrder = listCheckingOrder;
        }

        public List<OrdInfoModel> CurrentListOdr { get; private set; }
        public List<OrdInfoModel> ListCheckingOrder { get; private set; }
    }
}
