using Domain.Models.LockInf;
using Domain.Models.ReceptionLock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Reception.Get;
using UseCase.VisitingList.ReceptionLock;

namespace Interactor.VisitingList
{
    public class GetReceptionLockInteractor : IGetReceptionLockInputPort
    {
        private readonly IReceptionLockRepository _receptionLockRepository;

        public GetReceptionLockInteractor(IReceptionLockRepository receptionLockRepository)
        {
            _receptionLockRepository = receptionLockRepository;
        }

        public GetReceptionLockOutputData Handle(GetReceptionLockInputData inputData)
        {
            if (inputData.RaiinNo <= 0)
            {
                return new GetReceptionLockOutputData(new List<ReceptionLockModel>(), GetReceptionLockStatus.InvalidRaiinNo);
            }

            var listData = _receptionLockRepository.ReceptionLockModel(inputData.SinDate, inputData.PtId, inputData.RaiinNo, inputData.FunctionCd);
            if (listData == null || listData.Count == 0)
            {
                return new GetReceptionLockOutputData(new(), GetReceptionLockStatus.NoData);
            }
            return new GetReceptionLockOutputData(listData, GetReceptionLockStatus.Success);
        }
    }
}
