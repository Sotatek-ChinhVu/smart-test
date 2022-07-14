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
    public class PatientInforRepository 
    {
        private TenantDataContext _tenantDataContext;
        public PatientInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<PatientInfor> GetAll()
        {
            return _tenantDataContext.PtInfs.Select(x => ConvertToModel(x)).ToList();
        }

        private PatientInfor ConvertToModel(PtInf item)
        {
            PatientInfor PatientInfor = new PatientInfor(item.HpId, item.HpId, item.ReferenceNo, item.SeqNo, item.PtNum, item.KanaName, item.KanaName);
            return PatientInfor;
        }
    }
}
