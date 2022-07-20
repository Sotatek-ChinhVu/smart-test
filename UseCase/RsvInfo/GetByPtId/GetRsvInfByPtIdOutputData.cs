using Domain.Models.RsvInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInformation.GetById
{
    public class GetRsvInfByPtIdOutputData : IOutputData
    {
        public RsvInfModel? RsvInfModel { get; private set; }
        public GetRsvInfByPtIdOutputData(RsvInfModel? data)
        {
            RsvInfModel = data;
        }
    }
}