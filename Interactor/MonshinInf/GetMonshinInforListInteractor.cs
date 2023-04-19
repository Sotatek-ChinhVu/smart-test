using Domain.Models.MonshinInf;
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
                var monshins = _monshinInforRepository.GetMonshinInfor(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.IsDeleted, inputData.IsGetAll);

                if (!monshins.Any())
                {
                    return new GetMonshinInforListOutputData(new(), GetMonshinInforListStatus.NoData);
                }
                return new GetMonshinInforListOutputData(monshins, GetMonshinInforListStatus.Success);
            }
            finally
            {
                _monshinInforRepository.ReleaseResource();
            }
        }
    }
}
