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
                    if (item.HpId <= 0)
                        return new SaveMonshinOutputData(SaveMonshinStatus.InvalidHpId);
                    if (item.PtId <= 0)
                        return new SaveMonshinOutputData(SaveMonshinStatus.InvalidPtId);
                    if (item.RaiinNo <= 0)
                        return new SaveMonshinOutputData(SaveMonshinStatus.InvalidRaiinNo);
                    if (item.SinDate <= 0)
                        return new SaveMonshinOutputData(SaveMonshinStatus.InvalidSinDate);
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
