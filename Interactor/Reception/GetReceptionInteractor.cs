using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Reception.Get;

namespace Interactor.Reception
{
    public class GetReceptionInteractor : IGetReceptionInputPort
    {
        private readonly IReceptionRepository _receptionRepository;
        public GetReceptionInteractor(IReceptionRepository receptionRepository)
        {
            _receptionRepository = receptionRepository;
        }

        public GetReceptionOutputData Handle(GetReceptionInputData inputData)
        {
            if (inputData.RaiinNo <= 0)
            {
                return new GetReceptionOutputData(null, GetReceptionStatus.InvalidRaiinNo);
            }
            
            var receptionModel = _receptionRepository.Get(inputData.RaiinNo);
            if (receptionModel == null)
            {
                return new GetReceptionOutputData(null, GetReceptionStatus.ReceptionNotExisted);
            }

            return new GetReceptionOutputData(receptionModel, GetReceptionStatus.Successed);
        }
    }
}
