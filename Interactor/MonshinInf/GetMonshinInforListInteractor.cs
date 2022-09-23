using Domain.Models.MonshinInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MonshinInfor.GetList;

namespace Interactor.MonshinInf
{
    public class GetMonshinInforListInteractor : IGetMonshinInforListInputPort
    {
        private readonly IMonshinInforRepository _monshinInforRepository;

        public GetMonshinInforListInteractor(IMonshinInforRepository monshinInforRepository)
        {
            _monshinInforRepository = monshinInforRepository;
        }

        public GetMonshinInforListOutputData Handle(GetMonshinInforListInputData inputData)
        {
            try
            {
                var listMonshin = _monshinInforRepository.MonshinInforModels(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.IsDeleted);
                
                if (listMonshin == null || listMonshin.Count == 0)
                {
                    return new GetMonshinInforListOutputData(new(), GetMonshinInforListStatus.NoData);
                }
                return new GetMonshinInforListOutputData(listMonshin, GetMonshinInforListStatus.Success);
            }
            catch (Exception)
            {
                return new GetMonshinInforListOutputData(new List<MonshinInforModel>(), GetMonshinInforListStatus.Failed);
            }
        }
    }
}
