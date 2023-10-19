using Domain.Models.MedicalExamination;
using Domain.Models.MstItem;
using UseCase.UpsertMaterialMaster;

namespace Interactor.MedicalExamination
{
    public class UpsertMaterialMasterInteractor : IUpsertMaterialMasterInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpsertMaterialMasterInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public UpsertMaterialMasterOutputData Handle(UpsertMaterialMasterInputData inputData)
        {
            try
            {
                if(_mstItemRepository.UpsertMaterialMaster(inputData.HpId, inputData.UserId, inputData.MaterialMasters))
                {
                    return new UpsertMaterialMasterOutputData(UpsertMaterialMasterStatus.Success);
                }    
                return new UpsertMaterialMasterOutputData(UpsertMaterialMasterStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
