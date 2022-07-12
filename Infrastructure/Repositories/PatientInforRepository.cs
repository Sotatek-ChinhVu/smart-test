using Domain.Models.PatientInfor;
using Domain.Models.User;
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
            var allData = _tenantDataContext.PtInfs.ToList();
            return (IEnumerable<PatientInfor>)allData;
        }
    }
}
