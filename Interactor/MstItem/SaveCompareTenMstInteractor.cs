using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MstItem.CompareTenMst;
using UseCase.MstItem.SaveCompareTenMst;

namespace Interactor.MstItem
{
    public class SaveCompareTenMstInteractor : ISaveCompareTenMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public SaveCompareTenMstInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }

        public SaveCompareTenMstOutputData Handle(SaveCompareTenMstInputData inputData)
        {
            try
            {
                if (inputData.ListData.Count <= 0)
                {
                    return new SaveCompareTenMstOutputData(false, SaveCompareTenMstStatus.ListDataEmpty);
                }

                if(inputData.UserId <= 0)
                {
                    return new SaveCompareTenMstOutputData(false, SaveCompareTenMstStatus.InvalidUserId);
                }

                var result = _mstItemRepository.SaveCompareTenMst(inputData.ListData, inputData.Comparison, inputData.UserId);
                return new SaveCompareTenMstOutputData(result, result ? SaveCompareTenMstStatus.Success : SaveCompareTenMstStatus.Faild);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
