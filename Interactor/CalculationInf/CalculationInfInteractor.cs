using Domain.CalculationInf;
using Domain.Models.CalculationInf;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.CalculationInf;
using UseCase.Diseases.GetDiseaseList;
using UseCase.PatientInformation.GetById;

namespace Interactor.CalculationInf
{
    public class CalculationInfInteractor : ICalculationInfInputPort
    {
        private readonly ICalculationInfRepository _calculationInfRepository;
        public CalculationInfInteractor(ICalculationInfRepository calculationInfRepository)
        {
            _calculationInfRepository = calculationInfRepository;
        }
        public CalculationInfOutputData Handle(CalculationInfInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new CalculationInfOutputData(new List<CalculationInfModel>(), CalculationInfStatus.InvalidHpId);
                }
                if (inputData.PtId < 0)
                {
                    return new CalculationInfOutputData(new List<CalculationInfModel>(), CalculationInfStatus.InvalidPtId);
                }
                var listData = _calculationInfRepository.GetListDataCalculationInf(inputData.HpId, inputData.PtId);
                if (!listData.Any())
                {
                    return new CalculationInfOutputData(new List<CalculationInfModel>(), CalculationInfStatus.DataNotExist);
                }
                return new CalculationInfOutputData(listData.ToList(), CalculationInfStatus.Successed);
            }
            finally
            {
                _calculationInfRepository.ReleaseResource();
            }
        }
    }
}
