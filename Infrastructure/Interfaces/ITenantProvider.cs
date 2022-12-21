using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ITenantProvider
    {
        string GetConnectionString();

        string GetTenantInfo();
       
        string GetClinicID();

        TenantNoTrackingDataContext GetNoTrackingDataContext();

        TenantDataContext GetTrackingTenantDataContext();

        void ReloadNoTrackingDataContext();

        void ReloadTrackingDataContext();

        void DisposeDataContext();
    }
}
