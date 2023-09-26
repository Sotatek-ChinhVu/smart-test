using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MstItem.GetMaterialMsts;

namespace Interactor.MstItem
{
    public class GetMaterialMstsInteractor : IGetMaterialMstsInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetMaterialMstsInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetMaterialMstsOutputData Handle(GetMaterialMstsInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetMaterialMsts(inputData.HpId);
                if (data.Count == 0)
                {
                    return new GetMaterialMstsOutputData(new(), GetMaterialMstsStatus.NoData);
                }
                else
                {
                    return new GetMaterialMstsOutputData(data, GetMaterialMstsStatus.Success);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
