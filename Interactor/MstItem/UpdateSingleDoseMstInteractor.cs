using Domain.Models.MstItem;
using UseCase.MstItem.UpdateSingleDoseMst;

namespace Interactor.MstItem
{
    public class UpdateSingleDoseMstInteractor : IUpdateSingleDoseMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpdateSingleDoseMstInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }
        public UpdateSingleDoseMstOutputData Handle(UpdateSingleDoseMstInputData input)
        {
            try
            {
                var updateSingleDoseMst = _mstItemRepository.UpdateSingleDoseMst(input.HpId, input.UserId, input.SingleDoseMsts);
                return new UpdateSingleDoseMstOutputData(true);
            }
            catch
            {
                return new UpdateSingleDoseMstOutputData(false);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
