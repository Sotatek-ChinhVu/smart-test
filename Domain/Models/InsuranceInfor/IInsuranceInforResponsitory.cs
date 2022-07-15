using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceInfor
{
    public interface IInsuranceInforResponsitory
    {
        InsuranceInforModel? GetInsuranceInfor(PtId ptId, int HokenId);
    }
}
