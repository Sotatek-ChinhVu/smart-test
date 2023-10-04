using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MstItem.SaveCompareTenMst;
using UseCase.MstItem.SaveSetNameMnt;

namespace Interactor.MstItem
{
    public class SaveSetNameMntInteractor : ISaveSetNameMntInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public SaveSetNameMntInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }

        public SaveSetNameMntOutputData Handle(SaveSetNameMntInputData inputData)
        {
            try
            {
                if (inputData.ListData.Count <= 0)
                {
                    return new SaveSetNameMntOutputData(false, SaveSetNameMntStatus.ListDataEmpty);
                }

                if (inputData.HpId <= 0)
                {
                    return new SaveSetNameMntOutputData(false, SaveSetNameMntStatus.InvalidHpId);
                }

                if (inputData.UserId <= 0)
                {
                    return new SaveSetNameMntOutputData(false, SaveSetNameMntStatus.InvalidUserId);
                }

                if (inputData.Sindate <= 0)
                {
                    return new SaveSetNameMntOutputData(false, SaveSetNameMntStatus.InvalidSinDate);
                }

                var result = _mstItemRepository.SaveSetNameMnt(inputData.ListData, inputData.UserId, inputData.HpId, inputData.Sindate);
                return new SaveSetNameMntOutputData(result, result ? SaveSetNameMntStatus.Success : SaveSetNameMntStatus.Faild);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
