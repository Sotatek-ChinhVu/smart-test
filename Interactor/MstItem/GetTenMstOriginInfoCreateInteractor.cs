using Domain.Models.MstItem;
using Helper.Enum;
using UseCase.MstItem.GetTenMstOriginInfoCreate;

namespace Interactor.MstItem
{
    public class GetTenMstOriginInfoCreateInteractor : IGetTenMstOriginInfoCreateInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetTenMstOriginInfoCreateInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }

        public GetTenMstOriginInfoCreateOutputData Handle(GetTenMstOriginInfoCreateInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetTenMstOriginInfoCreateOutputData(GetTenMstOriginInfoCreateStatus.InvalidHpId, string.Empty, 0);

                ItemTypeEnums[] listTypeItemValid = new[]
                {
                    ItemTypeEnums.UsageItem,
                    ItemTypeEnums.JihiItem,
                    ItemTypeEnums.SpecificMedicalMeterialItem,
                    ItemTypeEnums.COCommentItem,
                    ItemTypeEnums.SpecialMedicineCommentItem,
                    ItemTypeEnums.KonikaItem,
                    ItemTypeEnums.FCRItem,
                    ItemTypeEnums.CombinedContraindicationItem,
                    ItemTypeEnums.Jibaiseki,
                    ItemTypeEnums.Shimadzu,
                };

                if (!listTypeItemValid.Contains(inputData.Type))
                    return new GetTenMstOriginInfoCreateOutputData(GetTenMstOriginInfoCreateStatus.InvalidTypeItem, string.Empty, 0);

                string startWithstr = TenMstMaintenanceUtil.GetStartWithByItemType(inputData.Type);

                string itemCd = _mstItemRepository.GetMaxItemCdByTypeForAdd(startWithstr);
                int jihiSbt = 0;

                if (inputData.Type == ItemTypeEnums.JihiItem)
                    jihiSbt = _mstItemRepository.GetMinJihiSbtMst(inputData.HpId);

                return new GetTenMstOriginInfoCreateOutputData(GetTenMstOriginInfoCreateStatus.Successful, itemCd, jihiSbt);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
