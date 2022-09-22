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
            try
            {
                var data = _receptionVisitingRepository.GetReceptionVisiting(inputData.HpId, inputData.RaiinNo);
                if (data == null)
                {
                    return new GetReceptionVisitingOutputData(GetReceptionVisitingStatus.NoData);
                }
                return new GetReceptionVisitingOutputData(data, GetReceptionVisitingStatus.Success);
            }
            catch (Exception)
            {

                return new GetReceptionVisitingOutputData(GetReceptionVisitingStatus.Failed);
            }

        }
    }
}
