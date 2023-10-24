using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MstItem.UpdateJihiSbtMst;
using UseCase.MstItem.UpdateSingleDoseMst;

namespace Interactor.MstItem
{
    public class UpdateJihiSbtMstInteractor : IUpdateJihiSbtMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpdateJihiSbtMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public UpdateJihiSbtMstOutputData Handle(UpdateJihiSbtMstInputData input)
        {
            try
            {
                if(!input.JihiSbtMsts.Any())
                {
                    return new UpdateJihiSbtMstOutputData(false);
                }
                var updateSingleDoseMst = _mstItemRepository.UpdateJihiSbtMst(input.HpId, input.UserId, input.JihiSbtMsts);
                return new UpdateJihiSbtMstOutputData(true);
            }
            catch
            {
                return new UpdateJihiSbtMstOutputData(false);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
