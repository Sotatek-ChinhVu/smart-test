using Domain.Models.Diseases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Diseases.GetDiseaseList;

namespace Interactor.Diseases
{
    public class GetPtDiseaseListInteractor : IGetPtDiseaseListInputPort
    {
        private readonly IPtDiseaseRepository _diseaseRepository;
        public GetPtDiseaseListInteractor(IPtDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
        }
        public GetPtDiseaseListOutputData Handle(GetPtDiseaseListInputData inputData)
        {
            try
            {
                var ptDiseaseListModel = _diseaseRepository.GetPatientDiseaseList(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HokenId, inputData.RequestFrom).ToList();
                if (!ptDiseaseListModel.Any())
                {
                    return new GetPtDiseaseListOutputData(new List<PtDiseaseModel>(), GetPtDiseaseListStatus.PtDiseaseListNotExisted);
                }
                return new GetPtDiseaseListOutputData(ptDiseaseListModel, GetPtDiseaseListStatus.Success);
            }
            finally
            {
                _diseaseRepository.ReleaseResource();
            }
        }
    }
}
