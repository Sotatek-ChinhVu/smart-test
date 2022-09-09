using Domain.Models.ReceptionVisitingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.ReceptionVisiting.Get;

namespace Interactor.ReceptionVisiting
{
    public class GetReceptionVisitingInteractor : IGetReceptionVisitingInputPort
    {
        private readonly IReceptionVisitingRepository _receptionVisitingRepository;
        public GetReceptionVisitingInteractor(IReceptionVisitingRepository receptionVisitingRepository)
        {
            _receptionVisitingRepository = receptionVisitingRepository;
        }

        public GetReceptionVisitingOutputData Handle(GetReceptionVisitingInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetReceptionVisitingOutputData(new List<ReceptionVisitingModel>(), GetReceptionVisitingStatus.InvalidHpId);
            }
            
            if (inputData.PtId <= 0)
            {
                return new GetReceptionVisitingOutputData(new List<ReceptionVisitingModel>(), GetReceptionVisitingStatus.InvalidPtId);
            }

            if (inputData.SinDate <= 0)
            {
                return new GetReceptionVisitingOutputData(new List<ReceptionVisitingModel>(), GetReceptionVisitingStatus.InvalidSinDate);
            }

            var listData = _receptionVisitingRepository.GetReceptionVisiting(inputData.HpId, inputData.PtId, inputData.SinDate);
            
            return new GetReceptionVisitingOutputData(listData.ToList(), GetReceptionVisitingStatus.Success);
        }
    }
}
