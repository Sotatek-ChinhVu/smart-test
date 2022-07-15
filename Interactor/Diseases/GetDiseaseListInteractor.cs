using Domain.CommonObject;
using Domain.Models.Diseases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Diseases.GetDiseaseList;

namespace Interactor.Diseases
{
    public class GetDiseaseListInteractor : IGetDiseaseListInputPort
    {
        private readonly IDiseaseRepository _ptByomeiRepository;
        public GetDiseaseListInteractor(IDiseaseRepository ptByomeiRepository)
        {
            _ptByomeiRepository = ptByomeiRepository;
        }
        public GetDiseaseListOutputData Handle(GetDiseaseListInputData inputData)
        {
            GetDiseaseListOutputData diseaseList = new GetDiseaseListOutputData(_ptByomeiRepository.GetAll(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RequestFrom).ToList());
            return diseaseList;
        }
    }
}
