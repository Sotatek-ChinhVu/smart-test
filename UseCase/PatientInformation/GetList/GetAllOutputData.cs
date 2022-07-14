using Domain.Models.PatientInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInformation.GetList
{
    public class GetAllOutputData: IOutputData
    {
        public List<PatientInfor> ListPatientInfor { get; private set; }

        public GetAllOutputData(List<PatientInfor> listData)
        {
            ListPatientInfor = listData;
        }
    }
}
