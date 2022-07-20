using Domain.Models.RaiinCmtInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.RaiinCmtInf;

namespace Interactor.RaiinCmtInf
{
    internal class GetRaiinCmtInfInteractor : IGetRaiinCmtInfInputPort
    {
        private readonly IRaiinCmtInfRepository _raiinCmtInfRepository;

        public GetRaiinCmtInfInteractor(IRaiinCmtInfRepository raiinCmtInfRepository)
        {
            _raiinCmtInfRepository = raiinCmtInfRepository;
        }

        public GetRaiinCmtInfOutputData Handle(GetRaiinCmtInfInputData inputData)
        {
            var data = _raiinCmtInfRepository.GetRaiinCmtInf(inputData.PtId, inputData.RaiinNo, inputData.SinDate);
            return new GetRaiinCmtInfOutputData(data);
        }
    }
}
