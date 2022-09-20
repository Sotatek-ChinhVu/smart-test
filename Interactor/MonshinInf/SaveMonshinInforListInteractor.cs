using Domain.Models.ColumnSetting;
using Domain.Models.MonshinInf;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MonshinInfor.Insert;
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
            bool success = _monshinInforRepository.SaveList(inputData.MonshinInfors, inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);
            var status = success ? SaveMonshinStatus.Success : SaveMonshinStatus.Success;
            return new SaveMonshinOutputData(status);
        }

    }
}
