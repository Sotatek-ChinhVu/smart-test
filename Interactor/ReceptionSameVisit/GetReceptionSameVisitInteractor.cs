using Domain.Models.ReceptionSameVisit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.ReceptionSameVisit.Get;

namespace Interactor.ReceptionSameVisit
{
    public class GetReceptionSameVisitInteractor : IGetReceptionSameVisitInputPort
    {
        private readonly IReceptionSameVisitRepository _receptionSameVisitRepository;
        public GetReceptionSameVisitInteractor(IReceptionSameVisitRepository receptionSameVisitRepository)
        {
            _receptionSameVisitRepository = receptionSameVisitRepository;
        }

        public GetReceptionSameVisitOutputData Handle(GetReceptionSameVisitInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetReceptionSameVisitOutputData(new List<ReceptionSameVisitModel>(), GetReceptionSameVisitStatus.InvalidHpId);
            }
            
            if (inputData.PtId <= 0)
            {
                return new GetReceptionSameVisitOutputData(new List<ReceptionSameVisitModel>(), GetReceptionSameVisitStatus.InvalidPtId);
            }

            if (inputData.SinDate <= 0)
            {
                return new GetReceptionSameVisitOutputData(new List<ReceptionSameVisitModel>(), GetReceptionSameVisitStatus.InvalidSinDate);
            }

            var listData = _receptionSameVisitRepository.GetReceptionSameVisit(inputData.HpId, inputData.PtId, inputData.SinDate);
            
            return new GetReceptionSameVisitOutputData(listData.ToList(), GetReceptionSameVisitStatus.Success);
        }
    }
}
