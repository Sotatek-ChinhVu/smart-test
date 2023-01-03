using Domain.Common;
using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionInsurance
{
    public interface IReceptionInsuranceRepository : IRepositoryBase
    {
        IEnumerable<ReceptionInsuranceModel> GetReceptionInsurance(int hpId, long ptId, int sinDate, bool isShowExpiredReception);

        string HasElderHoken(int sinDate, int hpId, long ptId, int ptInfBirthday);

        string IsValidAgeCheck(int sinDate, int hokenPid, int hpId, long ptId, int ptInfBirthday);
    }
}
