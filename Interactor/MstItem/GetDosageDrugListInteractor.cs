using Domain.Models.MstItem;
using Infrastructure.Repositories;
using UseCase.MstItem.GetDosageDrugList;

namespace Interactor.MstItem
{
    public class GetDosageDrugListInteractor : IGetDosageDrugListInputPort
    {
        private readonly IMstItemRepository _inputItemRepository;

        public GetDosageDrugListInteractor(IMstItemRepository inputItemRepository)
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
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
