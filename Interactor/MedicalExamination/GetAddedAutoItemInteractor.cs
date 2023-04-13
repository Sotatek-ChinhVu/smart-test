using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.GetAddedAutoItem;

namespace Interactor.MedicalExamination
{
    public class GetAddedAutoItemInteractor : IGetAddedAutoItemInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;
        public GetAddedAutoItemInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public GetAddedAutoItemOutputData Handle(GetAddedAutoItemInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetAddedAutoItemOutputData(GetAddedAutoItemStatus.InvalidHpId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetAddedAutoItemOutputData(GetAddedAutoItemStatus.InvalidSinDate, new());
                }
                if (inputData.PtId < 0)
                {
                    return new GetAddedAutoItemOutputData(GetAddedAutoItemStatus.InvalidPtId, new());
                }
                if (inputData.OrderInfItems.Count == 0)
                {
                    return new GetAddedAutoItemOutputData(GetAddedAutoItemStatus.InvalidAddedAutoItem, new());
                }

                var result = _todayOdrRepository.GetAutoAddOrders(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.OrderInfItems.Select(o => new Tuple<int, int, string>(o.OrderDetailPosition, o.OrderDetailPosition, o.ItemCd)).ToList(), inputData.CurrentOrderInfs.Select(c => new Tuple<int, int, string, double, int>(c.OrderPosition, c.OrderDetailPosition, c.ItemCd, c.Suryo, c.IsDeleted)).ToList());


                return new GetAddedAutoItemOutputData(GetAddedAutoItemStatus.Successed, result.Select(r => new AddedAutoItem(r.Item1, r.Item2, r.Item3.Select(i3 => new AddedOrderDetail(i3.Item1, i3.Item2, i3.Item3)).ToList())).ToList());
            }
            catch
            {
                return new GetAddedAutoItemOutputData(GetAddedAutoItemStatus.Failed, new());
            }
        }
    }
}
