using Domain.Models.SystemConf;
using UseCase.SystemConf.SavePath;

namespace Interactor.SystemConf
{
    public class SavePathInteractor : ISavePathInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;

        public SavePathInteractor(ISystemConfRepository systemConfRepository)
        {
            _systemConfRepository = systemConfRepository;
        }

        public SavePathOutputData Handle(SavePathInputData inputData)
        {
            try
            {
                var result = _systemConfRepository.SavePathConfOnline(inputData.HpId, inputData.UserId, inputData.SystemConfListXmlPathModels);
                if (result)
                {
                    return new SavePathOutputData(SavePathStatus.Successed);
                }

                return new SavePathOutputData(SavePathStatus.Failed);
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
            }
        }
    }
}
