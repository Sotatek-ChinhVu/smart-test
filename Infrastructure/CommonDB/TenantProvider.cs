using PostgreDataContext.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CommonDB
{
    public class TenantProvider : ITenantProvider
    {
        public string GetConnectionString()
        {
            return "host=localhost;port=5432;database=Emr;user id=postgres;password=Emr!23";
        }
    }
}
