using Domain.Models.MstItem;
using UseCase.MstItem.FindTenMst;

namespace Interactor.MstItem
{
    public class FindTenMstInteractor : IFindTenMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public FindTenMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public FindTenMstOutputData Handle(FindTenMstInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new FindTenMstOutputData(new TenItemModel(), FindTenMstStatus.InValidHpId);
            }

            if (inputData.SinDate <= 0)
            {
                return new FindTenMstOutputData(new TenItemModel(), FindTenMstStatus.InvalidSindate);
            }

            if (inputData.ItemCd.Trim().Length == 0 || inputData.ItemCd.Trim().Length > 10)
            {
                return new FindTenMstOutputData(new TenItemModel(), FindTenMstStatus.InvalidItemCd);
            }

            var data = _mstItemRepository.FindTenMst(inputData.HpId, inputData.ItemCd, inputData.SinDate);

            return new FindTenMstOutputData(data, FindTenMstStatus.Successed);
        }
    }
}
