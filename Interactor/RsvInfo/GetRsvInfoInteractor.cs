using Domain.Models.RsvInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.PatientInformation.GetById;

namespace Interactor.RsvInfo
{
    public class GetRsvInfoInteractor : IGetRsvInfByPtIdInputPort
    {
        private readonly IRsvInfRepository _rsvInfRepository;
        public GetRsvInfoInteractor(IRsvInfRepository rsvInfRepository)
        {
            _rsvInfRepository = rsvInfRepository;
        }

        public GetRsvInfByPtIdOutputData Handle(GetRsvInfByPtIdInputData inputData)
        {
            var data = _rsvInfRepository.GetRsvInfModel(inputData.PtId, inputData.SinDate);
            return new GetRsvInfByPtIdOutputData(data);
        }
    }
}
