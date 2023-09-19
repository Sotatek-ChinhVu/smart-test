using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.ContainerMasterUpdate;
using UseCase.MstItem.IsKensaItemOrdering;

namespace Interactor.MstItem
{
    public class IsKensaItemOrderingInteractor : IIsKensaItemOrderingInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public IsKensaItemOrderingInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public IsKensaItemOrderingOutputData Handle(IsKensaItemOrderingInputData inputData)
        {
            try
            {
                var result = _mstItemRepository.IsKensaItemOrdering(inputData.HpId, inputData.TenItemCd);
                if (result)
                {
                    return new IsKensaItemOrderingOutputData(IsKensaItemOrderingStatus.Success);
                }
                return new IsKensaItemOrderingOutputData(IsKensaItemOrderingStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
