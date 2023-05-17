using Domain.Models.MstItem;
using UseCase.MstItem.GetTenMstListByItemType;

namespace Interactor.MstItem
{
    public class GetTenMstListByItemTypeInteractor : IGetTenMstListByItemTypeInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetTenMstListByItemTypeInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetTenMstListByItemTypeOutputData Handle(GetTenMstListByItemTypeInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetTenMstListByItemTypeOutputData(GetTenMstListByItemTypeStatus.InvalidHpId, new List<TenMstMaintenanceModel>());

                if (inputData.SinDate <= 0)
                    return new GetTenMstListByItemTypeOutputData(GetTenMstListByItemTypeStatus.InvalidSinDate, new List<TenMstMaintenanceModel>());

                string startWithstr = TenMstMaintenanceUtil.GetStartWithByItemType(inputData.ItemType);

                var datas = _mstItemRepository.GetTenMstListByItemType(inputData.HpId, inputData.ItemType, startWithstr, inputData.SinDate);
                if(datas.Any())
                {
                    return new GetTenMstListByItemTypeOutputData(GetTenMstListByItemTypeStatus.Successful, datas);
                }
                else
                {
                    return new GetTenMstListByItemTypeOutputData(GetTenMstListByItemTypeStatus.NoData, datas);
                }   
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
