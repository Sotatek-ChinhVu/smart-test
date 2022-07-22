using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public interface IInsuranceRepository
    {
        InsuranceModel? GetInsuranceById(int hpId, long ptId, int sinDate, int hokenPid);
        IEnumerable<InsuranceModel> GetInsuranceListById(int hpId, long ptId, int SinDate);
    }
}
