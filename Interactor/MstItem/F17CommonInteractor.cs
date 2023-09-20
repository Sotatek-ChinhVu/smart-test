﻿using Domain.Models.MstItem;
using UseCase.IsUsingKensa;

namespace Interactor.MstItem
{
    public class F17CommonInteractor : IF17CommonInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public F17CommonInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public F17CommonOutputData Handle(F17CommonInputData inputData)
        {
            try
            {
                if (inputData.ItemCd != "")
                {
                    var result = _mstItemRepository.GetTenOfKNItem(inputData.HpId, inputData.ItemCd);
                    return new F17CommonOutputData(new(), F17CommonStatus.Success, new(), new(), new(), new(), new(), new(), result);
                }
                else if (inputData.TenItemCd != "")
                {
                    var result = _mstItemRepository.IsKensaItemOrdering(inputData.HpId, inputData.TenItemCd);
                    if (result)
                    {
                        return new F17CommonOutputData(new(), F17CommonStatus.Success, new(), new(), new(), new(), new(), new(), 0);
                    }
                    return new F17CommonOutputData(new(), F17CommonStatus.Failed, new(), new(), new(), new(), new(), new(), 0);
                }
                else if (inputData.KensaStdItemCd != "")
                {
                    var data = _mstItemRepository.GetKensaStdMstModels(inputData.HpId, inputData.KensaStdItemCd);
                    if (data.Count == 0)
                    {
                        return new F17CommonOutputData(new(), F17CommonStatus.NoData, new(), new(), new(), new(), new(), new(), 0);
                    }
                    else
                    {
                        return new F17CommonOutputData(new(), F17CommonStatus.Success, data, new(), new(), new(), new(), new(), 0);
                    }
                }
                else
                {
                    var kensaItemCds = _mstItemRepository.GetUsedKensaItemCds(inputData.HpId);
                    var itemCds = _mstItemRepository.GetTenItemCds(inputData.HpId);
                    var materialMsts = _mstItemRepository.GetMaterialMsts(inputData.HpId);
                    var containerMsts = _mstItemRepository.GetContainerMsts(inputData.HpId);
                    var kensaCenterMsts = _mstItemRepository.GetKensaCenterMsts(inputData.HpId);
                    var tenOfItem = _mstItemRepository.GetTenOfItem(inputData.HpId);
                    if (kensaItemCds.Count == 0 || itemCds.Count == 0)
                    {
                        return new F17CommonOutputData(new(), F17CommonStatus.NoData, new(), new(), new(), new(), new(), new(), 0);
                    }
                    else
                    {
                        return new F17CommonOutputData(kensaItemCds, F17CommonStatus.Success, new(), itemCds, materialMsts, containerMsts, kensaCenterMsts, tenOfItem, 0);
                    }
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
