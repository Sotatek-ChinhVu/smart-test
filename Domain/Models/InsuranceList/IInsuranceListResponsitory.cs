using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceList
{
    public interface IInsuranceListResponsitory
    {
        IEnumerable<InsuranceListModel> GetInsuranceListById(PtId ptId);
    }
}
