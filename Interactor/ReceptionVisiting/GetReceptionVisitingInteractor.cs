using Domain.Models.KarteInfs;
using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.KarteInfs.GetLists;
using UseCase.ReceptionVisiting.Get;

namespace Interactor.ReceptionVisiting
{
    public class GetReceptionVisitingInteractor : IGetReceptionVisitingInputPort
    {
        private readonly IReceptionRepository _receptionVisitingRepository;
        public GetReceptionVisitingInteractor(IReceptionRepository receptionVisitingRepository)
        {
            _receptionVisitingRepository = receptionVisitingRepository;
        }

        public GetReceptionVisitingOutputData Handle(GetReceptionVisitingInputData inputData)
        {
            if (inputData.RaiinNo <= 0)
            {
                return new GetReceptionVisitingOutputData(new List<ReceptionModel>(), GetReceptionVisitingStatus.InvalidRaiinNo);
            }
            
            var listData = _receptionVisitingRepository.GetReceptionVisiting(inputData.RaiinNo);
            if (listData == null || listData.Count == 0)
            {
                return new GetReceptionVisitingOutputData(new(), GetReceptionVisitingStatus.NoData);
            }
            return new GetReceptionVisitingOutputData(listData, GetReceptionVisitingStatus.Success);
        }
    }
}
