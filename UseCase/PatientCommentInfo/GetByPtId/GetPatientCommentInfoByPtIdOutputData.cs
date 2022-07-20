using Domain.Models.PatientCommentInfo;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientCommentInfo.GetById
{
    public class GetPatientCommentInfoByPtIdOutputData : IOutputData
    {
        public PatientCmtInfModel? PatientCmtInfModel { get; private set; }
        public GetPatientCommentInfoByPtIdOutputData(PatientCmtInfModel? data)
        {
            PatientCmtInfModel = data;
        }
    }
}