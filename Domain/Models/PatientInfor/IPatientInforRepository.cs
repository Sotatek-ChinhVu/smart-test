using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientInfor
{
    public interface IPatientInforRepository
    {
        IEnumerable<PatientInfor> GetAll();
        PatientInfor? GetById(PtId ptId);
    }
}
