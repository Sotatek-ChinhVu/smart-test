using Domain.Models.KarteInfs;
using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.KarteInf.GetList;
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
                if (inputData.HpId <= 0)
                    return new GetReceptionVisitingOutputData(GetReceptionVisitingStatus.InvalidHpId);

                if (inputData.RaiinNo <= 0)
                    return new GetReceptionVisitingOutputData(GetReceptionVisitingStatus.InvalidRaiinNo);

                var data = _receptionVisitingRepository.GetReceptionVisiting(inputData.HpId, inputData.RaiinNo);

                return new GetReceptionVisitingOutputData(data, GetReceptionVisitingStatus.Success);
            }
            catch (Exception)
            {
                return new GetReceptionVisitingOutputData(GetReceptionVisitingStatus.Failed);
            }
            finally
            {
                _receptionVisitingRepository.ReleaseResource();
            }
        }
    }
}
