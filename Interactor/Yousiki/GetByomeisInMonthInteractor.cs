using Domain.Models.Diseases;
using Domain.Models.Yousiki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Yousiki.GetByomeisInMonth;

namespace Interactor.Yousiki
{
    public class GetByomeisInMonthInteractor : IGetByomeisInMonthInputPort
    {
        private readonly IPtDiseaseRepository _ptDiseaseRepository;

        public GetByomeisInMonthInteractor(IPtDiseaseRepository ptDiseaseRepository)
        {
            _ptDiseaseRepository = ptDiseaseRepository;
        }
        public GetByomeisInMonthOutputData Handle(GetByomeisInMonthInputData inputData)
        {
            try
            {
                var result = _ptDiseaseRepository.GetByomeisInMonth(inputData.HpId, inputData.PtId, inputData.SinYm);
                return new GetByomeisInMonthOutputData(result, GetByomeisInMonthStatus.Successed);
            }
            finally
            {
                _ptDiseaseRepository.ReleaseResource();
            }
        }
    }
}
