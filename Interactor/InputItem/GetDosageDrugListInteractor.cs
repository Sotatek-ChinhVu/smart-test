using Domain.Models.InputItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.InputItem.GetDosageDrugList;

namespace Interactor.InputItem
{
    public class GetDosageDrugListInteractor : IGetDosageDrugListInputPort
    {
        private readonly IInputItemRepository _inputItemRepository;

        public GetDosageDrugListInteractor(IInputItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public GetDosageDrugListOutputData Handle(GetDosageDrugListInputData inputData)
        {
            try
            {
                if (inputData.ToList().Count == 0)
                {
                    return new GetDosageDrugListOutputData(new List<DosageDrugModel>(), GetDosageDrugListStatus.InputDataNull);
                }

                var datas = _inputItemRepository.GetDosages(inputData.YjCds);
                if (!(datas?.Count > 0))
                {
                    return new GetDosageDrugListOutputData(new List<DosageDrugModel>(), GetDosageDrugListStatus.NoData);
                }

                return new GetDosageDrugListOutputData(datas, GetDosageDrugListStatus.Successed);
            }
            catch
            {
                return new GetDosageDrugListOutputData(new List<DosageDrugModel>(), GetDosageDrugListStatus.Fail);
            }
        }
    }
}
