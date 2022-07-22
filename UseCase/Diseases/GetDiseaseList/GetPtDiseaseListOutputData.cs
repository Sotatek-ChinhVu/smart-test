﻿using Domain.Models.Diseases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetDiseaseList
{
    public class GetPtDiseaseListOutputData : IOutputData
    {
        public List<PtDiseaseModel> DiseaseList { get; private set; }

        public GetPtDiseaseListStatus Status { get; private set; }

        public GetPtDiseaseListOutputData(List<PtDiseaseModel> diseaseList, GetPtDiseaseListStatus status)
        {
            DiseaseList = diseaseList;
            Status = status;
        }

    }
}
