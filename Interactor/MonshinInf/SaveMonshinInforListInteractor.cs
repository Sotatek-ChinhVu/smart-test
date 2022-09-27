using Domain.Models.MonshinInf;
using UseCase.MonshinInfor.Save;

namespace Interactor.MonshinInf
{
    public class SaveMonshinInforListInteractor : ISaveMonshinInputPort
    {
        private readonly IMonshinInforRepository _monshinInforRepository;

        public SaveMonshinInforListInteractor(IMonshinInforRepository monshinInforRepository)
        {
            _monshinInforRepository = monshinInforRepository;
        }

        public SaveMonshinOutputData Handle(SaveMonshinInputData inputData)
        {
            try
            {
                if (!inputData.MonshinInfors.Any())
                {
                    return new SaveMonshinOutputData(SaveMonshinStatus.InputDataNull);
                }
                foreach (var item in inputData.MonshinInfors)
                {
                    if (!_monshinInforRepository.CheckExistMonshinInf(item.HpId, item.PtId, item.SinDate, item.RaiinNo))
                        return new SaveMonshinOutputData(SaveMonshinStatus.InputDataDoesNotExists);
                }
                bool success = _monshinInforRepository.SaveList(inputData.MonshinInfors);
                var status = success ? SaveMonshinStatus.Success : SaveMonshinStatus.Failed;
                return new SaveMonshinOutputData(status);
            }
            catch (Exception)
            {
                return new SaveMonshinOutputData(SaveMonshinStatus.Failed);
            }
        }
    }
}
