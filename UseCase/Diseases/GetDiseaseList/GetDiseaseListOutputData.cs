using Domain.Models.Diseases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetDiseaseList
{
    public class GetDiseaseListOutputData : IOutputData
    {
        public List<Disease> DiseaseList { get; private set; }

        public GetDiseaseListOutputData(List<Disease> diseaseList)
        {
            DiseaseList = diseaseList;
        }

    }
}
