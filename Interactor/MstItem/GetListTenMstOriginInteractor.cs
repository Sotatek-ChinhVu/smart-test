using Domain.Models.MstItem;
using UseCase.MstItem.GetListTenMstOrigin;

namespace Interactor.MstItem
{
    public class GetListTenMstOriginInteractor : IGetListTenMstOriginInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetListTenMstOriginInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }

        public GetListTenMstOriginOutputData Handle(GetListTenMstOriginInputData inputData)
        {
            try
            {
                if (string.IsNullOrEmpty(inputData.ItemCd))
                    return new GetListTenMstOriginOutputData(new List<TenMstOriginModel>(), GetListTenMstOriginStatus.InvalidItemCd);

                var tenMstModelLists = _mstItemRepository.GetGroupTenMst(inputData.HpId, inputData.ItemCd);
                if (!tenMstModelLists.Any())
                    return new GetListTenMstOriginOutputData(tenMstModelLists, GetListTenMstOriginStatus.NoData);
                else
                    return new GetListTenMstOriginOutputData(tenMstModelLists, GetListTenMstOriginStatus.Successful);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
