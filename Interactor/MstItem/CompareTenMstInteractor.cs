using Domain.Enum;
using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MstItem.CompareTenMst;

namespace Interactor.MstItem
{
    public class CompareTenMstInteractor: ICompareTenMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public CompareTenMstInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }

        public CompareTenMstOutputData Handle(CompareTenMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new CompareTenMstOutputData(new List<CompareTenMstModel>(), CompareTenMstStatus.InvalidHpId);
                }
                
                if (inputData.SinDate <= 0)
                {
                    return new CompareTenMstOutputData(new List<CompareTenMstModel>(), CompareTenMstStatus.InvalidSindate);
                }

                var result = _mstItemRepository.SearchCompareTenMst(inputData.HpId, inputData.SinDate, inputData.Actions, inputData.Comparions);
                return new CompareTenMstOutputData(result, CompareTenMstStatus.Success);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
