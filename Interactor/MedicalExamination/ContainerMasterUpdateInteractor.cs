using Domain.Models.MedicalExamination;
using Domain.Models.MstItem;
using UseCase.ContainerMasterUpdate;

namespace Interactor.MedicalExamination
{
    public class ContainerMasterUpdateInteractor : IContainerMasterUpdateInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public ContainerMasterUpdateInteractor(IMstItemRepository mstItemRepository) 
        {
            _mstItemRepository = mstItemRepository;
        }
        public ContainerMasterUpdateOutPutData Handle(ContainerMasterUpdateInputData inputData)
        {
            try
            {
                if(_mstItemRepository.ContainerMasterUpdate(inputData.HpId, inputData.UserId, inputData.ContainerMasters))
                {
                    return new ContainerMasterUpdateOutPutData(ContainerMasterUpdateStatus.Successful);
                }
                return new ContainerMasterUpdateOutPutData(ContainerMasterUpdateStatus.Failed);
            }
            finally 
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
