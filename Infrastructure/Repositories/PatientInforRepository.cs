using Domain.Models.PatientInfor;
using Domain.Models.User;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PatientInforRepository: IPatientInforRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public PatientInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<PatientInfor> GetAll()
        {
            return _tenantDataContext.PtInfs.Select(x => new PatientInfor(x.HpId, x.HpId, x.ReferenceNo, x.SeqNo, x.PtNum, x.KanaName, x.KanaName)).Take(5);
        }

    }
}
